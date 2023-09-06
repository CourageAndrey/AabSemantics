using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Statements;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class StatementsConstructorTest
	{
		private const string TestStatementId = "123";

		#region ProcessesStatement

		[Test]
		public void GivenNoProcessA_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, null, processB, sign));
		}

		[Test]
		public void GivenNoProcessB_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, processB, null));
		}

		[Test]
		public void GivenProcessAWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenProcessBWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		#endregion

		#region Common properties (ID, name, hint)

		[Test]
		public void GivenBasicStatements_WhenCheckHint_ThenItIsNotEmpty()
		{
		// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new ProcessesModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsProcessAttribute.Value });
			var concept2 = 2.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsProcessAttribute.Value });
			var concept3 = 3.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsSequenceSignAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new ProcessesStatement(null, concept1, concept2, concept3),
			})
			{
				Assert.IsNotNull(statement.Hint);
				Assert.IsNotNull(statement.Hint.GetValue(language));
			}
		}

		#endregion
	}
}
