using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class CustomStatementTest
	{
		[SetUp]
		public void SetUp()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
		}

		[Test]
		public void GivenCustomProcessesStatement_WhenDescribe_ThenReturnTheSameAsOriginal()
		{
			// arrange
			var a = "a".CreateConceptByName().WithAttribute(IsProcessAttribute.Value);
			var b = "b".CreateConceptByName().WithAttribute(IsProcessAttribute.Value);

			// act
			var statement = new ProcessesStatement(null, a, b, SequenceSigns.Causes);
			var custom = statement.ToCustomStatement();

			// assert
			statement.CheckAllDescriptionsAgainst(custom, "Processes");
		}
	}
}
