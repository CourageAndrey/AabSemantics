using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
{
	[TestFixture]
	public class ComparisonStatementTest
	{
		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			// arrange
			IConcept	left = 1.CreateConceptByObject().WithAttribute(IsValueAttribute.Value),
						right = 2.CreateConceptByObject().WithAttribute(IsValueAttribute.Value);
			var statement = new ComparisonStatement(null, left, right, ComparisonSigns.IsLessThan);

			// act && assert
			Assert.IsFalse(statement.Equals(null));
		}
	}
}
