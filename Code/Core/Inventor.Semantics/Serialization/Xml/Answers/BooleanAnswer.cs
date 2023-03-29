using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Xml.Answers
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
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public BooleanAnswer(Semantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver)
		{
			return new Semantics.Answers.BooleanAnswer(
				Result,
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.Save(conceptIdResolver))));
		}
	}
}