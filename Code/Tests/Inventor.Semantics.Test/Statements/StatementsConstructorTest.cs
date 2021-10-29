using System;
using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Mathematics.Attributes;
using Inventor.Semantics.Mathematics.Statements;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Processes.Statements;
using Inventor.Semantics.Set.Attributes;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Test.Statements
{
	[TestFixture]
	public class StatementsConstructorTest
	{
		private const string TestStatementId = "123";

		#region HasPartStatement

		[Test]
		public void GivenNoWholeWhenTryToCreateHasPartStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoPartWhenTryToCreateHasPartStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, concept, null));
		}

		#endregion

		#region GroupStatement

		[Test]
		public void GivenNoAreaWhenTryToCreateGroupStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoConceptWhenTryToCreateGroupStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, concept, null));
		}

		#endregion

		#region HasSignStatement

		[Test]
		public void GivenNoConceptWhenTryToCreateHasSignStatementThenFail()
		{
			// arrange
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, null, sign));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateHasSignStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, concept, null));
		}

		[Test]
		public void GivenSignWithoutAttributeWhenTryToCreateHasSignStatementThenFail()
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
		public void GivenNoAncestorWhenTryToCreateIsStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoDescendantWhenTryToCreateIsStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, concept, null));
		}

		#endregion

		#region SignValueStatement

		[Test]
		public void GivenNoConceptWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, null, sign, value));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateSignValueStatementThenFail()
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
		public void GivenNoValueWhenTryToCreateSignValueStatementThenFail()
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
		public void GivenSignWithoutAttributeWhenTryToCreateSignValueStatementThenFail()
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
		public void GivenValueWithoutAttributeWhenTryToCreateSignValueStatementThenFail()
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
		public void GivenNoLeftValueWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenNoRightValueWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenNoSignWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenLeftWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenRightWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenSignWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
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
		public void GivenNoProcessAWhenTryToCreateProcessesStatementThenFail()
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
		public void GivenNoProcessBWhenTryToCreateProcessesStatementThenFail()
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
		public void GivenNoSignWhenTryToCreateProcessesStatementThenFail()
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
		public void GivenProcessAWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
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
		public void GivenProcessBWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
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
		public void GivenSignWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
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
		public void CreateStatementWithAllPropertiesSet()
		{
			// arrange && act
			var statement = new TestStatement(TestStatementId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.AreEqual(TestStatementId, statement.ID);
		}

		[Test]
		public void CreateStatementWithoutId()
		{
			// arrange && act
			var statement = new TestStatement(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.IsNotNullOrEmpty(statement.ID);
		}

		[Test]
		public void FailToCreateStatementWithoutName()
		{
			Assert.Throws<ArgumentNullException>(() => new TestStatement(TestStatementId, null, new LocalizedStringVariable()));
		}

		[Test]
		public void CreateStatementWithoutHint()
		{
			// arrange && act
			var statement = new TestStatement(TestStatementId, new LocalizedStringVariable(), null);

			// assert
			Assert.IsNotNull(statement.Hint);
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
