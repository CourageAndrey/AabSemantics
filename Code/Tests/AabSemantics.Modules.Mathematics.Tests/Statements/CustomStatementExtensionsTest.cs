using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Set;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
{
	[TestFixture]
	public class CustomStatementExtensionsTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
		}

		[Test]
		public void GivenComparisonStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var leftValue = ConceptCreationHelper.CreateEmptyConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValue = ConceptCreationHelper.CreateEmptyConcept();
			rightValue.WithAttribute(IsValueAttribute.Value);

			var statement = new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsEqualTo);

			//var language = Language.Default;
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new SetModule().RegisterMetadata();

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToComparisonStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
			Assert.That(statement.DescribeTrue().ToString(), Is.EqualTo(restored.DescribeTrue().ToString()));
			Assert.That(statement.DescribeFalse().ToString(), Is.EqualTo(restored.DescribeFalse().ToString()));
			Assert.That(statement.DescribeQuestion().ToString(), Is.EqualTo(restored.DescribeQuestion().ToString()));
		}
	}
}
