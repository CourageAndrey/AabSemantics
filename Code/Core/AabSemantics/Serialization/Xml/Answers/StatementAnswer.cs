using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Localization;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Xml.Answers
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
			: base(AabSemantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public StatementAnswer(AabSemantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.StatementAnswer(
				Statement.SaveOrReuse(conceptIdResolver, statementIdResolver),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}