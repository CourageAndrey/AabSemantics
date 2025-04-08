using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
{
	[TestFixture]
	public class CustomStatementTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
		}

		[Test]
		public void GivenCustomComparisonStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var left = "l".CreateConceptByName().WithAttribute(IsValueAttribute.Value);
			var right = "r".CreateConceptByName().WithAttribute(IsValueAttribute.Value);

			// act
			var statement = new ComparisonStatement(null, left, right, ComparisonSigns.IsEqualTo);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Comparison");
		}
	}
}
