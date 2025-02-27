using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class CheckStatementQuestionTest
	{
		static CheckStatementQuestionTest()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			Repositories.Reset();
		}

		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new CheckStatementQuestion(null));
		}

		[Test]
		public void GivenIsTrueThat_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			var checkedStatement = semanticNetwork.Statements.First();

			// act
			var questionRegular = new CheckStatementQuestion(checkedStatement);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.Ask().IsTrueThat(checkedStatement);

			// assert
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenNoMatchingStatementsFound_WhenAsk_ThenReturnEmptyAnswer()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			RegisterTestStatements(semanticNetwork);

			var statementToCheck = new TestStatement(1);
			var statementWrong = new TestStatement(2);

			// act
			var answerNoStatements = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			semanticNetwork.Statements.Add(statementWrong);
			var answerWrongStatement = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			// assert
			var answer = (BooleanAnswer) answerNoStatements;
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			answer = (BooleanAnswer) answerWrongStatement;
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenExistingStatement_WhenAsk_ThenReturnFoundStatement()
		{
			// arrange
			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			var semanticNetwork = new SemanticNetwork(language);
			RegisterTestStatements(semanticNetwork);

			var statementToCheck = new TestStatement(1);
			var statementRight = new TestStatement(1);
			var statementWrong = new TestStatement(2);
			semanticNetwork.Statements.Add(statementRight);
			semanticNetwork.Statements.Add(statementWrong);

			// act
			var answerToCheck = semanticNetwork.Ask().IsTrueThat(statementToCheck);
			var textToCheck = (FormattedText) ((ITextContainer) answerToCheck.Description).Items.First();

			var answerRight = semanticNetwork.Ask().IsTrueThat(statementRight);
			var textRight = (FormattedText) ((ITextContainer) answerRight.Description).Items.First();

			// assert
			Assert.That(answerToCheck.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerToCheck).Result, Is.True);
			Assert.That(answerToCheck.Explanation.Statements.Single(), Is.SameAs(statementRight));
			Assert.That(textToCheck.ToString(), Is.EqualTo($"\"true\""));

			Assert.That(answerRight.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerRight).Result, Is.True);
			Assert.That(answerRight.Explanation.Statements.Single(), Is.SameAs(statementRight));
			Assert.That(textRight.ToString(), Is.EqualTo($"\"true\""));
		}

		[Test]
		public void GivenTransitiveStatements_WhenAsk_ThenProcessCorrectly()
		{
			// arrange
			var language = Language.Default;

			var semanticNetwork = new SemanticNetwork(language);
			RegisterTestStatements(semanticNetwork);

			var conceptParent = "parent".CreateConceptByName();
			var conceptIntermediate = "intermediate".CreateConceptByName();
			var conceptChild = "child".CreateConceptByName();
			semanticNetwork.Concepts.Add(conceptParent);
			semanticNetwork.Concepts.Add(conceptIntermediate);
			semanticNetwork.Concepts.Add(conceptChild);

			var statementPI = new TransitiveTestStatement(conceptParent, conceptIntermediate);
			var statementIC = new TransitiveTestStatement(conceptIntermediate, conceptChild);
			semanticNetwork.Statements.Add(statementPI);
			semanticNetwork.Statements.Add(statementIC);

			var statementToCheck = new TransitiveTestStatement(conceptParent, conceptChild);

			// act
			var answer = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answer.Explanation.Statements.Contains(statementPI), Is.True);
			Assert.That(answer.Explanation.Statements.Contains(statementIC), Is.True);
		}

		private static void RegisterTestStatements(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			Repositories.RegisterStatement(
				typeof(TestStatement),
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				s => new Dictionary<string, IKnowledge>(),
				StatementDefinition.NoConsistencyCheck);

			Repositories.RegisterStatement(
				typeof(TransitiveTestStatement),
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				l => string.Empty,
				s => new Dictionary<string, IKnowledge>(),
				StatementDefinition.NoConsistencyCheck);
		}

		private class TestStatement : TestCore.TestStatement
		{
			public int Number
			{ get; }

			public TestStatement(int number)
				: base(number.ToString(), LocalizedString.Empty, LocalizedString.Empty)
			{
				Number = number;
			}

			public override bool Equals(TestCore.TestStatement other)
			{
				return Number == ((TestStatement) other).Number;
			}
		}

		private class TransitiveTestStatement : TestCore.TestStatement, IParentChild<IConcept>
		{
			public IConcept Parent
			{ get; }

			public IConcept Child
			{ get; }

			public TransitiveTestStatement(IConcept parent, IConcept child)
				: base(string.Empty.EnsureIdIsSet(), LocalizedString.Empty, LocalizedString.Empty)
			{
				Parent = parent;
				Child = child;
			}

			public override bool Equals(TestCore.TestStatement other)
			{
				var typed = (TransitiveTestStatement) other;
				return Parent == typed.Parent && Child == typed.Child;
			}

			public override IEnumerable<IConcept> GetChildConcepts()
			{
				yield return Parent;
				yield return Child;
			}
		}
	}
}
