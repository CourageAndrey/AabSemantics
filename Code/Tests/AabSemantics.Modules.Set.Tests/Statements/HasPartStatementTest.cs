using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Set.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class HasPartStatementTest
	{
		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			// arrange
			IConcept	concept1 = 1.CreateConcept(),
						concept2 = 2.CreateConcept();
			var statement = new HasPartStatement(null, concept1, concept2);

			// act && assert
			Assert.IsFalse(statement.Equals(null));
		}
	}
}
