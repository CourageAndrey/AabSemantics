using System;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementsConstructorTest
	{
		private const string TestStatementId = "123";

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

		#region Common properties (ID, name, hint)

		[Test]
		public void GivenAllPropertiesSet_WhenCreate_ThenSucceed()
		{
			// arrange && act
			var statement = new TestStatement(TestStatementId, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.That(statement.ID, Is.EqualTo(TestStatementId));
		}

		[Test]
		public void GivenNoId_WhenCreate_ThenIdIsNotNull()
		{
			// arrange && act
			var statement = new TestStatement(null, new LocalizedStringVariable(), new LocalizedStringVariable());

			// assert
			Assert.That(string.IsNullOrEmpty(statement.ID), Is.False);
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
			Assert.That(statement.Hint, Is.Not.NaN);
		}

		[Test]
		public void GivenBasicStatements_WhenCheckHint_ThenItIsNotEmpty()
		{
		// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });
			var concept2 = 2.CreateConceptByObject().WithAttributes(new IAttribute[] { IsValueAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new IsStatement(null, concept1, concept2),
			})
			{
				Assert.That(statement.Hint, Is.Not.Null);
				Assert.That(statement.Hint.GetValue(language), Is.Not.Null);
			}
		}

		#endregion
	}
}
