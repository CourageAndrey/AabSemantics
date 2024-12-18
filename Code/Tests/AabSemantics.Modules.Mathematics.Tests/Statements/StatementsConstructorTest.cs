using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
{
	[TestFixture]
	public class StatementsConstructorTest
	{
		private const string TestStatementId = "123";

		#region ComparisonStatement

		[Test]
		public void GivenNoLeftValue_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var right = ConceptCreationHelper.CreateEmptyConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, null, right, sign));
		}

		[Test]
		public void GivenNoRightValue_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateEmptyConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, null, sign));
		}

		[Test]
		public void GivenNoSign_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateEmptyConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateEmptyConcept();
			right.WithAttribute(IsValueAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonStatement(TestStatementId, left, right, null));
		}

		[Test]
		public void GivenLeftWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateEmptyConcept();
			var right = ConceptCreationHelper.CreateEmptyConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenRightWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateEmptyConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept();
			sign.WithAttribute(IsComparisonSignAttribute.Value);

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
		}

		[Test]
		public void GivenSignWithoutAttribute_WhenTryToCreateComparisonStatement_ThenFail()
		{
			// arrange
			var left = ConceptCreationHelper.CreateEmptyConcept();
			left.WithAttribute(IsValueAttribute.Value);
			var right = ConceptCreationHelper.CreateEmptyConcept();
			right.WithAttribute(IsValueAttribute.Value);
			var sign = ConceptCreationHelper.CreateEmptyConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new ComparisonStatement(TestStatementId, left, right, sign));
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
				new MathematicsModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });
			var concept2 = 2.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });
			var concept3 = 3.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsComparisonSignAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new ComparisonStatement(null, concept1, concept2, concept3),
			})
			{
				Assert.That(statement.Hint, Is.Not.Null);
				Assert.That(statement.Hint.GetValue(language), Is.Not.Null);
			}
		}

		#endregion
	}
}
