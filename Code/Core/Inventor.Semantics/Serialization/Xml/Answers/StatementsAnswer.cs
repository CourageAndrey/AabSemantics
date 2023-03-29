using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class StatementsAnswer : Answer
	{
		#region Properties

		[XmlArray(nameof(Statements))]
		public List<Statement> Statements
		{ get; set; }

		#endregion

		#region Constructors

		public StatementsAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{
			Statements = new List<Statement>();
		}

		public StatementsAnswer(Semantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver)
		{
			return new Semantics.Answers.StatementsAnswer(
				Statements.Select(statement => statement.Save(conceptIdResolver)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.Save(conceptIdResolver))));
		}
	}
}