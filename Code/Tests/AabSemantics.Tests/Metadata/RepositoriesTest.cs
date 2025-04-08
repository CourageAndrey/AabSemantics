using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Statements;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class RepositoriesTest
	{
		[Test]
		public void GivenRepositories_WhenTryToSetNullModules_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Modules = null);

			Assert.DoesNotThrow(() => Repositories.Modules = new Dictionary<string, IExtensionModule>());
		}

		[Test]
		public void GivenRepositories_WhenTryToSetNullAttributes_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Attributes = null);

			Assert.DoesNotThrow(() => Repositories.Attributes = new Repository<AttributeDefinition>());
		}

		[Test]
		public void GivenRepositories_WhenTryToSetNullStatements_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Statements = null);

			Assert.DoesNotThrow(() => Repositories.Statements = new Repository<StatementDefinition>());
		}

		[Test]
		public void GivenRepositories_WhenTryToSetNullQuestions_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Questions = null);

			Assert.DoesNotThrow(() => Repositories.Questions = new Repository<QuestionDefinition>());
		}

		[Test]
		public void GivenRepositories_WhenTryToSetNullAnswers_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Answers = null);

			Assert.DoesNotThrow(() => Repositories.Answers = new Repository<AnswerDefinition>());
		}

		[Test]
		public void GivenRepositories_WhenTryToSetNullCustomStatements_ThenFail()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.CustomStatements = null);

			Assert.DoesNotThrow(() => Repositories.CustomStatements = new Dictionary<string, CustomStatementDefinition>());
		}

		[Test]
		public void GivenTypedAndUntypedMethods_WhenRegisterMetadata_ThenWorksTheSame()
		{
			// arrange
			Repositories.Attributes.Definitions.Clear();
			Repositories.Statements.Definitions.Clear();
			Repositories.Questions.Definitions.Clear();
			Repositories.Answers.Definitions.Clear();

			// act
			var attributeDefinition = Repositories.RegisterAttribute(typeof(IsValueAttribute), IsValueAttribute.Value, l => string.Empty);
			var statementDefinition = Repositories.RegisterStatement(
				typeof(IsStatement),
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				s => new Dictionary<string, IKnowledge>(),
				(semanticNetwork, result) => { });
			var questionDefinition = Repositories.RegisterQuestion(typeof(CheckStatementQuestion), l => string.Empty);
			var answerDefinition = Repositories.RegisterAnswer(typeof(BooleanAnswer));

			Repositories.Attributes.Definitions.Clear();
			Repositories.Statements.Definitions.Clear();
			Repositories.Questions.Definitions.Clear();
			Repositories.Answers.Definitions.Clear();

			var attributeDefinitionT = Repositories.RegisterAttribute<IsValueAttribute>(IsValueAttribute.Value, l => string.Empty);
			var statementDefinitionT = Repositories.RegisterStatement<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				l => string.Empty,
				s => new Dictionary<string, IKnowledge>(),
				(statements, result, semanticNetwork) => { });
			var questionDefinitionT = Repositories.RegisterQuestion<CheckStatementQuestion>(l => string.Empty);
			var answerDefinitionT = Repositories.RegisterAnswer<BooleanAnswer>();

			// assert
			Assert.That(attributeDefinitionT.Type, Is.SameAs(attributeDefinition.Type));
			Assert.That(statementDefinition.Type, Is.SameAs(statementDefinitionT.Type));
			Assert.That(questionDefinitionT.Type, Is.SameAs(questionDefinition.Type));
			Assert.That(answerDefinition.Type, Is.SameAs(answerDefinitionT.Type));
		}
	}
}
