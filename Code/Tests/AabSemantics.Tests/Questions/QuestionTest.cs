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
			Assert.IsFalse(answerWithoutPreconditions.IsEmpty);
			Assert.IsFalse(answerWithoutPreconditions.Result);
			Assert.AreEqual(0, answerWithoutPreconditions.Explanation.Statements.Count);

			Assert.IsFalse(answerWithPreconditions.IsEmpty);
			Assert.IsTrue(answerWithPreconditions.Result);
			Assert.AreEqual(2, answerWithPreconditions.Explanation.Statements.Count);
			Assert.IsTrue(answerWithPreconditions.Explanation.Statements.Contains(initialStatement));
			Assert.IsTrue(answerWithPreconditions.Explanation.Statements.Contains(preconditionStatement));
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
			Assert.AreSame(question, childAnswer.Question);
			Assert.AreSame(answer, childAnswer.Answer);
			Assert.AreSame(transitives, childAnswer.TransitiveStatements);
		}
	}
}
