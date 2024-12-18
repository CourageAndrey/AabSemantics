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
	public class QuestionTest
	{
		[Test]
		public void GivenPreconditions_WhenBeingAsked_ThenQuestionIsProcessedTakenThemIntoAccount()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept parentConcept, middleConcept, childConcept;
			semanticNetwork.Concepts.Add(parentConcept = ConceptCreationHelper.CreateEmptyConcept());
			semanticNetwork.Concepts.Add(middleConcept = ConceptCreationHelper.CreateEmptyConcept());
			semanticNetwork.Concepts.Add(childConcept = ConceptCreationHelper.CreateEmptyConcept());

			var initialStatement = semanticNetwork.DeclareThat(parentConcept).IsAncestorOf(middleConcept);

			var preconditionStatement = new IsStatement(null, middleConcept, childConcept);
			// ... and do not add it to semantic network

			// act
			var answerWithoutPreconditions = (BooleanAnswer) semanticNetwork.Ask().IfIs(childConcept, parentConcept);
			var answerWithPreconditions = (BooleanAnswer) semanticNetwork.Supposing(new IStatement[] { preconditionStatement }).Ask().IfIs(childConcept, parentConcept);

			// assert
			Assert.That(answerWithoutPreconditions.IsEmpty, Is.False);
			Assert.That(answerWithoutPreconditions.Result, Is.False);
			Assert.That(answerWithoutPreconditions.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerWithPreconditions.IsEmpty, Is.False);
			Assert.That(answerWithPreconditions.Result, Is.True);
			Assert.That(answerWithPreconditions.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answerWithPreconditions.Explanation.Statements.Contains(initialStatement), Is.True);
			Assert.That(answerWithPreconditions.Explanation.Statements.Contains(preconditionStatement), Is.True);
		}

		[Test]
		public void GivenAllParameterSet_WhenCreateChildAnswer_ThenSucceed()
		{
			// arrange
			var childConcept = "child".CreateConceptByName();
			var parentConcept = "parent".CreateConceptByName();
			var question = new IsQuestion(childConcept, parentConcept);
			var answer = Answer.CreateUnknown();
			var transitives = Array.Empty<IStatement>();

			// act
			var childAnswer = new ChildAnswer(question, answer, transitives);

			// assert
			Assert.That(childAnswer.Question, Is.SameAs(question));
			Assert.That(childAnswer.Answer, Is.SameAs(answer));
			Assert.That(childAnswer.TransitiveStatements, Is.SameAs(transitives));
		}
	}
}
