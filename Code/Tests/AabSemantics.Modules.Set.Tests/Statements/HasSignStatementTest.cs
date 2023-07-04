using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
{
	[TestFixture]
	public class HasSignStatementTest
	{
		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			// arrange
			IConcept	concept = 1.CreateConcept(),
						sign = 2.CreateConcept().WithAttribute(IsSignAttribute.Value);
			var statement = new HasSignStatement(null, concept, sign);

			// act && assert
			Assert.IsFalse(statement.Equals(null));
		}
	}
}
