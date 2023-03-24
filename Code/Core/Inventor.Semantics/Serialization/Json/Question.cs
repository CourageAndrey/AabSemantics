using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
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

		static Question()
		{
			var questionType = typeof(Question);
			var serializer = new DataContractJsonSerializer(
				questionType,
				Repositories.Statements.GetJsonTypes());
			questionType.DefineCustomJsonSerializer(serializer);
		}
	}

	[DataContract]
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
