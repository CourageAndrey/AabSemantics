using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			var checkedStatement = semanticNetwork.Statements.First();

			// act
			var questionRegular = new CheckStatementQuestion(checkedStatement);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.Ask().IsTrueThat(checkedStatement);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoMatchingStatementsFound_WhenAsk_ThenReturnEmptyAnswer()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			var statementToCheck = new TestStatement(1);
			var statementWrong = new TestStatement(2);

			// act
			var answerNoStatements = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			semanticNetwork.Statements.Add(statementWrong);
			var answerWrongStatement = semanticNetwork.Ask().IsTrueThat(statementToCheck);

			// assert
			var answer = (BooleanAnswer) answerNoStatements;
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(answer.Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);

			answer = (BooleanAnswer) answerWrongStatement;
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(answer.Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenExistingStatement_WhenAsk_ThenReturnFoundStatement()
		{
			// arrange
			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			var semanticNetwork = new SemanticNetwork(language);
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
			Assert.IsFalse(answerToCheck.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerToCheck).Result);
			Assert.AreSame(statementRight, answerToCheck.Explanation.Statements.Single());
			Assert.AreEqual($"\"true\"", textToCheck.ToString());

			Assert.IsFalse(answerRight.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerRight).Result);
			Assert.AreSame(statementRight, answerRight.Explanation.Statements.Single());
			Assert.AreEqual($"\"true\"", textRight.ToString());
		}

		[Test]
		public void GivenTransitiveStatements_WhenAsk_ThenProcessCorrectly()
		{
			// arrange
			var language = Language.Default;

			var semanticNetwork = new SemanticNetwork(language);
			var conceptParent = "parent".CreateConcept();
			var conceptIntermediate = "intermediate".CreateConcept();
			var conceptChild = "child".CreateConcept();
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
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.Contains(statementPI));
			Assert.IsTrue(answer.Explanation.Statements.Contains(statementIC));
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
