using System;
using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Statements
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
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoPartWhenTryToCreateHasPartStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, concept, null));
		}

		#endregion

		#region GroupStatement

		[Test]
		public void GivenNoAreaWhenTryToCreateGroupStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoConceptWhenTryToCreateGroupStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, concept, null));
		}

		#endregion

		#region HasSignStatement

		[Test]
		public void GivenNoConceptWhenTryToCreateHasSignStatementThenFail()
		{
			// arrange
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, null, sign));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateHasSignStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, concept, null));
		}

		[Test]
		public void GivenSignWithoutAttributeWhenTryToCreateHasSignStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new HasSignStatement(TestStatementId, concept, sign));
		}

		#endregion

		#region IsStatement

		[Test]
		public void GivenNoAncestorWhenTryToCreateIsStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoDescendantWhenTryToCreateIsStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, concept, null));
		}

		#endregion

		#region SignValueStatement

		[Test]
		public void GivenNoConceptWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = TestHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, null, sign, value));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = TestHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, null, value));
		}

		[Test]
		public void GivenNoValueWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = TestHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, sign, null));
		}

		[Test]
		public void GivenSignWithoutAttributeWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			var value = TestHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
		}

		[Test]
		public void GivenValueWithoutAttributeWhenTryToCreateSignValueStatementThenFail()
		{
			// arrange
			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
		}

		#endregion

		#region ComparisonStatement

		[Test]
		public void GivenNoLeftValueWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var right = TestHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, null, right, sign));
		}

		[Test]
		public void GivenNoRightValueWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var left = TestHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, null, sign));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var left = TestHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = TestHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, right, null));
		}

		[Test]
		public void GivenLeftWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var left = TestHelper.CreateConcept();
			var right = TestHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenRightWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var left = TestHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenSignWithoutAttributeWhenTryToCreateComparisonStatementThenFail()
		{
			// arrange
			var left = TestHelper.CreateConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = TestHelper.CreateConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = TestHelper.CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		#endregion

		#region ProcessesStatement

		[Test]
		public void GivenNoProcessAWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processB = TestHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, null, processB, sign));
		}

		[Test]
		public void GivenNoProcessBWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processA = TestHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, null, sign));
		}

		[Test]
		public void GivenNoSignWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processA = TestHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = TestHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesStatement(TestStatementId, processA, processB, null));
		}

		[Test]
		public void GivenProcessAWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processA = TestHelper.CreateConcept();
			var processB = TestHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenProcessBWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processA = TestHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSequenceSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ProcessesStatement(TestStatementId, processA, processB, sign));
		}

		[Test]
		public void GivenSignWithoutAttributeWhenTryToCreateProcessesStatementThenFail()
		{
			// arrange
			var processA = TestHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = TestHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			var sign = TestHelper.CreateConcept();

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

			protected override string GetDescriptionText(ILanguageStatements language)
			{
				throw new NotImplementedException();
			}

			protected override IDictionary<string, INamed> GetDescriptionParameters()
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
