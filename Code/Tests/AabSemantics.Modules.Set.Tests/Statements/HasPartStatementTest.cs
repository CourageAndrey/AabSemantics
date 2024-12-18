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
			IConcept	concept1 = 1.CreateConceptByObject(),
						concept2 = 2.CreateConceptByObject();
			var statement = new HasPartStatement(null, concept1, concept2);

			// act && assert
			Assert.That(statement.Equals(null), Is.False);
		}

		[Test]
		public void GivenHasPartStatement_WhenGetParentAndChild_ThenEqualToWholeAndPart()
		{
			// arrange
			IConcept	whole = 1.CreateConceptByObject(),
						part = 2.CreateConceptByObject();
			var statement = new HasPartStatement(null, whole, part);

			// act && assert
			Assert.That(statement.Parent, Is.SameAs(whole));
			Assert.That(statement.Child, Is.SameAs(part));
		}
	}
}
