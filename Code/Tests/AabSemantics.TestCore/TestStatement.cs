using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.TestCore
{
	public class TestStatement : Statement<TestStatement>
	{
		public TestStatement()
			: this(null, LocalizedString.Empty, LocalizedString.Empty)
		{ }

		public TestStatement(string id, LocalizedString name, LocalizedString hint = null)
			: base(id, name, hint)
		{ }

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			return Array.Empty<IConcept>();
		}

		protected override string GetDescriptionTrueText(ILanguage language)
		{
			return string.Empty;
		}

		protected override string GetDescriptionFalseText(ILanguage language)
		{
			return string.Empty;
		}

		protected override string GetDescriptionQuestionText(ILanguage language)
		{
			return string.Empty;
		}

		protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<string, IKnowledge>();
		}

		public override bool Equals(TestStatement other)
		{
			return ReferenceEquals(this, other);
		}
	}
}
