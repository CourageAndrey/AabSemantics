using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementsConstructorTest
	{
		private const string TestStatementId = "123";

		#region HasPartStatement

		[Test]
		public void GivenNoWhole_WhenTryToCreateHasPartStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoPart_WhenTryToCreateHasPartStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, concept, null));
		}

		#endregion

		#region GroupStatement

		[Test]
		public void GivenNoArea_WhenTryToCreateGroupStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoConcept_WhenTryToCreateGroupStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, concept, null));
		}

		#endregion

		#region HasSignStatement

		[Test]
		public void GivenNoConcept_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, concept, null));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new HasSignStatement(TestStatementId, concept, sign));
		}

		#endregion

		#region IsStatement

		[Test]
		public void GivenNoAncestor_WhenTryToCreateIsStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoDescendant_WhenTryToCreateIsStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, concept, null));
		}

		#endregion

		#region SignValueStatement

		[Test]
		public void GivenNoConcept_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, null, sign, value));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, null, value));
		}

		[Test]
		public void GivenNoValue_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, sign, null));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
		}

		[Test]
		public void GivenValueWithoutAttribute_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
		}

		#endregion

		#region ComparisonStatement

		[Test]
		public void GivenNoLeftValue_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var right = ConceptCreationHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, null, right, sign));
		}

		[Test]
		public void GivenNoRightValue_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, right, null));
		}

		[Test]
		public void GivenLeftWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateConcept();
			var right = ConceptCreationHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenRightWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		#endregion

		#region ProcessesStatement

		[Test]
		public void GivenNoProcessA_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processB = ConceptCreationHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, null, processB, sign));
		}

		[Test]
		public void GivenNoProcessB_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, processB, null));
		}

		[Test]
		public void GivenProcessAWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateConcept();
			var processB = ConceptCreationHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenProcessBWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateProcessesStatement_ThenFail()
		{
			// arrange
			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		#endregion

		#region Common properties (ID, name, hint)

		[Test]
		public void GivenAllPropertiesSet_WhenCreate_ThenSucceed()
		{
			// arrange && act
			var statement = new TestStatement(TestStatementId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.AreEqual(TestStatementId, statement.ID);
		}

		[Test]
		public void GivenNoId_WhenCreate_ThenIdIsNotNull()
		{
			// arrange && act
			var statement = new TestStatement(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.IsFalse(string.IsNullOrEmpty(statement.ID));
		}

		[Test]
		public void GivenNoName_WhenTryToCreate_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => new TestStatement(TestStatementId, null, new LocalizedStringVariable()));
		}

		[Test]
		public void GivenNoHint_WhenCreate_ThenHintIsNotNull()
		{
			// arrange && act
			var statement = new TestStatement(TestStatementId, new LocalizedStringVariable(), null);

			// assert
			Assert.IsNotNull(statement.Hint);
		}

		[Test]
		public void GivenBasicStatements_WhenCheckHint_ThenItIsNotEmpty()
		{
		// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new SetModule(),
				new MathematicsModule(),
				new ProcessesModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConcept().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsProcessAttribute.Value });
			var concept2 = 2.CreateConcept().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsProcessAttribute.Value, IsSignAttribute.Value });
			var concept3 = 3.CreateConcept().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsComparisonSignAttribute.Value, IsSequenceSignAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new HasPartStatement(null, concept1, concept2),
				new GroupStatement(null, concept1, concept2),
				new HasSignStatement(null, concept1, concept2),
				new IsStatement(null, concept1, concept2),
				new SignValueStatement(null, concept1, concept2, concept3),
				new ComparisonStatement(null, concept1, concept2, concept3),
				new ProcessesStatement(null, concept1, concept2, concept3),
			})
			{
				Assert.IsNotNull(statement.Hint);
				Assert.IsNotNull(statement.Hint.GetValue(language));
			}
		}

		#endregion

		private class TestStatement : Statement
		{
			public TestStatement(string id, ILocalizedString name, ILocalizedString hint)
				: base(id, name, hint)
			{ }

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				throw new NotImplementedException();
			}

			protected override String GetDescriptionTrueText(ILanguage language)
			{
				throw new NotImplementedException();
			}

			protected override String GetDescriptionFalseText(ILanguage language)
			{
				throw new NotImplementedException();
			}

			protected override String GetDescriptionQuestionText(ILanguage language)
			{
				throw new NotImplementedException();
			}

			protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
			{
				throw new NotImplementedException();
			}

			public override bool CheckUnique(IEnumerable<IStatement> statements)
			{
				throw new NotImplementedException();
			}

			public override bool Equals(object obj)
			{
				throw new NotImplementedException();
			}
		}
	}
}
