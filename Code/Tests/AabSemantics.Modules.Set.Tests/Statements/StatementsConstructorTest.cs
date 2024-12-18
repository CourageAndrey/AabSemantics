using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;

namespace AabSemantics.Modules.Set.Tests.Statements
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
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoPart_WhenTryToCreateHasPartStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasPartStatement(TestStatementId, concept, null));
		}

		#endregion

		#region GroupStatement

		[Test]
		public void GivenNoArea_WhenTryToCreateGroupStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoConcept_WhenTryToCreateGroupStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GroupStatement(TestStatementId, concept, null));
		}

		#endregion

		#region HasSignStatement

		[Test]
		public void GivenNoConcept_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignStatement(TestStatementId, concept, null));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateHasSignStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new HasSignStatement(TestStatementId, concept, sign));
		}

		#endregion

		#region IsStatement

		[Test]
		public void GivenNoAncestor_WhenTryToCreateIsStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, null, concept));
		}

		[Test]
		public void GivenNoDescendant_WhenTryToCreateIsStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsStatement(TestStatementId, concept, null));
		}

		#endregion

		#region SignValueStatement

		[Test]
		public void GivenNoConcept_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, null, sign, value));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, null, value));
		}

		[Test]
		public void GivenNoValue_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueStatement(TestStatementId, concept, sign, null));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			var value = ConceptCreationHelper.CreateEmptyConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
		}

		[Test]
		public void GivenValueWithoutAttribute_WhenTryToCreateSignValueStatement_ThenFail()
		{
			// arrange
			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new SignValueStatement(TestStatementId, concept, sign, value));
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
				new SetModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });
			var concept2 = 2.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsSignAttribute.Value });
			var concept3 = 3.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new HasPartStatement(null, concept1, concept2),
				new GroupStatement(null, concept1, concept2),
				new HasSignStatement(null, concept1, concept2),
				new SignValueStatement(null, concept1, concept2, concept3),
			})
			{
				Assert.That(statement.Hint, Is.Not.Null);
				Assert.That(statement.Hint.GetValue(language), Is.Not.Null);
			}
		}

		#endregion
	}
}
