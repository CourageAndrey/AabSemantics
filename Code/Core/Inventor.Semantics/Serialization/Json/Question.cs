using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[Serializable]
	public abstract class Question
	{
		#region Properties

		public List<Statement> Preconditions
		{ get; set; }

		#endregion

		#region Constructors

		protected Question()
		{
			Preconditions = new List<Statement>();
		}

		protected Question(IQuestion question)
		{
			Preconditions = question.Preconditions.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		public static Question Load(IQuestion question)
		{
			var definition = Repositories.Questions.Definitions.GetSuitable(question);
			return definition.GetJson(question);
		}

		public abstract IQuestion Save(ConceptIdResolver conceptIdResolver);
	}

	[Serializable]
	public abstract class Question<QuestionT> : Question
		where QuestionT : IQuestion
	{
		#region Constructors

		protected Question()
			: base()
		{ }

		protected Question(QuestionT question)
			: base(question)
		{ }

		#endregion

		public override IQuestion Save(ConceptIdResolver conceptIdResolver)
		{
			return SaveImplementation(
				conceptIdResolver,
				Preconditions.Select(statement => statement.Save(conceptIdResolver)));
		}

		protected abstract QuestionT SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions);
	}
}
