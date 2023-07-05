using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;

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
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var concept1 = 1.CreateConcept().WithAttributes(new IAttribute[] { IsValueAttribute.Value });
			var concept2 = 2.CreateConcept().WithAttributes(new IAttribute[] { IsValueAttribute.Value });

			var language = Language.Default;

			// act && assert
			foreach (var statement in new IStatement[]
			{
				new IsStatement(null, concept1, concept2),
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
