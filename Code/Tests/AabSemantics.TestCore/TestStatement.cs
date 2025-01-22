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

		public override bool Equals(TestStatement other)
		{
			return ReferenceEquals(this, other);
		}
	}
}
