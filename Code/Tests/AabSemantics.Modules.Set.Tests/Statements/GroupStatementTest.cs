using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Set.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class GroupStatementTest
	{
		[Test]
		public void GivenGroupStatement_WhenGetParentAndChild_ThenEqualToWholeAndPart()
		{
			// arrange
			IConcept	area = 1.CreateConceptByObject(),
						concept = 2.CreateConceptByObject();
			var statement = new GroupStatement(null, area, concept);

			// act && assert
			Assert.That(statement.Parent, Is.SameAs(area));
			Assert.That(statement.Child, Is.SameAs(concept));
		}

		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			// arrange
			IConcept	area = 1.CreateConceptByObject(),
						concept = 2.CreateConceptByObject();
			var statement = new GroupStatement(null, area, concept);

			// act && assert
			Assert.That(statement.Equals(null), Is.False);
		}
	}
}
