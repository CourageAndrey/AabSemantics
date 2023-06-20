using System;
using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Modules.Classification.Statements;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class RepositoriesTest
	{
		[Test]
		public void ImpossibleToSetNullModules()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Modules = null);

			Assert.DoesNotThrow(() => Repositories.Modules = new Dictionary<string, IExtensionModule>());
		}

		[Test]
		public void ImpossibleToSetNullAttributes()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Attributes = null);

			Assert.DoesNotThrow(() => Repositories.Attributes = new Repository<AttributeDefinition>());
		}

		[Test]
		public void ImpossibleToSetNullStatements()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Statements = null);

			Assert.DoesNotThrow(() => Repositories.Statements = new Repository<StatementDefinition>());
		}

		[Test]
		public void ImpossibleToSetNullQuestions()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Questions = null);

			Assert.DoesNotThrow(() => Repositories.Questions = new Repository<QuestionDefinition>());
		}

		[Test]
		public void ImpossibleToSetNullAnswers()
		{
			Assert.Throws<ArgumentNullException>(() => Repositories.Answers = null);

			Assert.DoesNotThrow(() => Repositories.Answers = new Repository<AnswerDefinition>());
		}

		[Test]
		public void TypedAnduntypedRegistrationWorkTheSame()
		{
			// arrange
			Repositories.Attributes.Definitions.Clear();
			Repositories.Statements.Definitions.Clear();
			Repositories.Questions.Definitions.Clear();
			Repositories.Answers.Definitions.Clear();

			// act
			var attributeDefinition = Repositories.RegisterAttribute(typeof(IsValueAttribute), IsValueAttribute.Value, l => string.Empty);
			var statementDefinition = Repositories.RegisterStatement(typeof(IsStatement), l => string.Empty, (semanticNetwork, result) => { });
			var questionDefinition = Repositories.RegisterQuestion(typeof(CheckStatementQuestion), l => string.Empty);
			var answerDefinition = Repositories.RegisterAnswer(typeof(BooleanAnswer));

			Repositories.Attributes.Definitions.Clear();
			Repositories.Statements.Definitions.Clear();
			Repositories.Questions.Definitions.Clear();
			Repositories.Answers.Definitions.Clear();

			var attributeDefinitionT = Repositories.RegisterAttribute<IsValueAttribute>(IsValueAttribute.Value, l => string.Empty);
			var statementDefinitionT = Repositories.RegisterStatement<IsStatement>(l => string.Empty, (statements, result, semanticNetwork) => { });
			var questionDefinitionT = Repositories.RegisterQuestion<CheckStatementQuestion>(l => string.Empty);
			var answerDefinitionT = Repositories.RegisterAnswer<BooleanAnswer>();

			// assert
			Assert.AreSame(attributeDefinition.Type, attributeDefinitionT.Type);
			Assert.AreSame(statementDefinition.Type, statementDefinitionT.Type);
			Assert.AreSame(questionDefinitionT.Type, questionDefinition.Type);
			Assert.AreSame(answerDefinition.Type, answerDefinitionT.Type);
		}
	}
}
