using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Localization;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Xml.Answers
{
	[XmlType]
	public class BooleanAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Boolean Result
		{ get; set; }

		#endregion

		#region Constructors

		public BooleanAnswer()
			: base(AabSemantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public BooleanAnswer(AabSemantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.BooleanAnswer(
				Result,
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}