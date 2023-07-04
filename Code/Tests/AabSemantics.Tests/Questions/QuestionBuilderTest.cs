using System;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class QuestionBuilderTest
	{
		[Test]
		public void GivenPreconditions_WhenBeingAsked_ThenTakeThemIntoAccount()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept ancestor, middle, descendant;
			semanticNetwork.Concepts.Add(ancestor = "ancestor".CreateConcept());
			semanticNetwork.Concepts.Add(middle = "middle".CreateConcept());
			semanticNetwork.Concepts.Add(descendant = "descendant".CreateConcept());

			var existingStatement = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(middle);
			var precondition = new IsStatement(null, middle, descendant);
			var preconditions = new IStatement[] { precondition };

			// act
			var questionRegularWithoutPreconditions = new IsQuestion(descendant, ancestor);
			var answerRegularWithoutPreconditions = (BooleanAnswer) questionRegularWithoutPreconditions.Ask(semanticNetwork.Context);

			var answerBuilderWithoutPreconditions = (BooleanAnswer) semanticNetwork.Ask().IfIs(descendant, ancestor);

			var questionRegularWithPreconditions = new IsQuestion(descendant, ancestor, preconditions);
			var answerRegularWithPreconditions = (BooleanAnswer) questionRegularWithPreconditions.Ask(semanticNetwork.Context);

			var answerBuilderWithPreconditions = (BooleanAnswer) semanticNetwork.Supposing(preconditions).Ask().IfIs(descendant, ancestor);

			// assert
			Assert.IsFalse(answerRegularWithoutPreconditions.Result);
			Assert.AreEqual(0, answerRegularWithoutPreconditions.Explanation.Statements.Count);
			Assert.IsFalse(answerBuilderWithoutPreconditions.Result);
			Assert.AreEqual(0, answerBuilderWithoutPreconditions.Explanation.Statements.Count);
			Assert.IsTrue(answerRegularWithPreconditions.Result);
			Assert.AreEqual(2, answerRegularWithPreconditions.Explanation.Statements.Count);
			Assert.IsTrue(answerRegularWithPreconditions.Result);
			Assert.IsTrue(answerRegularWithPreconditions.Explanation.Statements.Contains(existingStatement));
			Assert.IsTrue(answerRegularWithPreconditions.Explanation.Statements.Contains(precondition));
			Assert.AreEqual(2, answerBuilderWithPreconditions.Explanation.Statements.Count);
			Assert.IsTrue(answerBuilderWithPreconditions.Explanation.Statements.Contains(existingStatement));
			Assert.IsTrue(answerBuilderWithPreconditions.Explanation.Statements.Contains(precondition));
		}

		[Test]
		public void GivenNoSemanticNetwork_WhenBeingAsked_ThenThrowArgumentNullException()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionBuilder(null, Array.Empty<IStatement>()));
		}
	}
}
