using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Xml
{
	[XmlType]
	public abstract class Question
	{
		#region Properties

		[XmlArray(nameof(Preconditions))]
		public List<Statement> Preconditions
		{ get; set; } = new List<Statement>();

		#endregion

		public static Question Load(IQuestion question)
		{
			var definition = Repositories.Questions.Definitions.GetSuitable(question);
			return definition.GetXml(question);
		}

		public abstract IQuestion Save(ConceptIdResolver conceptIdResolver);
	}

	[XmlType]
	public abstract class Question<QuestionT> : Question
		where QuestionT : IQuestion
	{
		public override IQuestion Save(ConceptIdResolver conceptIdResolver)
		{
			return SaveImplementation(
				conceptIdResolver,
				Preconditions.Select(statement => statement.Save(conceptIdResolver)));
		}

		protected abstract QuestionT SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions);
	}
}
