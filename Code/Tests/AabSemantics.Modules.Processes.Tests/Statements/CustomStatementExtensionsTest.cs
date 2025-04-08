using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class CustomStatementExtensionsTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
		}

		[Test]
		public void GivenProcessesStatement_WhenConvertToCustomAndBack_ThenResultLooksEqual()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			var statement = new ProcessesStatement(null, processA, processB, SequenceSigns.Causes);

			// act
			var custom = statement.ToCustomStatement();
			var restored = custom.ToProcessesStatement();

			// assert
			Assert.That(statement, Is.EqualTo(restored));
		}
	}
}
