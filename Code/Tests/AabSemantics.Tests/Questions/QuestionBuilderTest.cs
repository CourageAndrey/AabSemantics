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
			semanticNetwork.Concepts.Add(ancestor = "ancestor".CreateConceptByName());
			semanticNetwork.Concepts.Add(middle = "middle".CreateConceptByName());
			semanticNetwork.Concepts.Add(descendant = "descendant".CreateConceptByName());

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
			Assert.That(answerRegularWithoutPreconditions.Result, Is.False);
			Assert.That(answerRegularWithoutPreconditions.Explanation.Statements.Count, Is.EqualTo(0));
			Assert.That(answerBuilderWithoutPreconditions.Result, Is.False);
			Assert.That(answerBuilderWithoutPreconditions.Explanation.Statements.Count, Is.EqualTo(0));
			Assert.That(answerRegularWithPreconditions.Result, Is.True);
			Assert.That(answerRegularWithPreconditions.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answerRegularWithPreconditions.Result, Is.True);
			Assert.That(answerRegularWithPreconditions.Explanation.Statements.Contains(existingStatement), Is.True);
			Assert.That(answerRegularWithPreconditions.Explanation.Statements.Contains(precondition), Is.True);
			Assert.That(answerBuilderWithPreconditions.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answerBuilderWithPreconditions.Explanation.Statements.Contains(existingStatement), Is.True);
			Assert.That(answerBuilderWithPreconditions.Explanation.Statements.Contains(precondition), Is.True);
		}

		[Test]
		public void GivenNoSemanticNetwork_WhenBeingAsked_ThenThrowArgumentNullException()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionBuilder(null, Array.Empty<IStatement>()));
		}
	}
}
