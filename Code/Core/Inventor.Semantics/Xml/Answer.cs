using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Xml.Answers;

namespace Inventor.Semantics.Xml
{
	[XmlType]
	[XmlInclude(typeof(BooleanAnswer))]
	[XmlInclude(typeof(ConceptAnswer))]
	[XmlInclude(typeof(ConceptsAnswer))]
	[XmlInclude(typeof(StatementAnswer))]
	[XmlInclude(typeof(StatementsAnswer))]
	public class Answer
	{
		#region Properties

		[XmlElement]
		public String Description
		{ get; }

		[XmlArray(nameof(Explanation))]
		public List<Statement> Explanation
		{ get; }

		[XmlElement]
		public Boolean IsEmpty
		{ get; }

		#endregion

		public Answer(IAnswer answer, ILanguage language)
		{
			Description = TextRepresenters.PlainString.Represent(answer.Description, language).ToString();
			Explanation = answer.Explanation.Statements.Select(statement => Statement.Load(statement)).ToList();
			IsEmpty = answer.IsEmpty;
		}

		public static Answer Load(IAnswer answer, ILanguage language)
		{
			var definition = Repositories.Answers.Definitions.GetSuitable(answer);
			return definition.GetXml(answer, language);
		}
	}
}
