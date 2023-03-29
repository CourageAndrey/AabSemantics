using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class StatementAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		public StatementAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public StatementAnswer(Semantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.StatementAnswer(
				Statement.SaveOrReuse(conceptIdResolver, statementIdResolver),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}