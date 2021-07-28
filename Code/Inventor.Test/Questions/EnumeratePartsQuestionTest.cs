using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class EnumeratePartsQuestionTest
	{
		[Test]
		public void WhenNoRelationshipsReturnEmptyAnswer()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var conceptToCheck = new Concept();
			var wholeConcept = new Concept();
			var partConcept = new Concept();
			knowledgeBase.Concepts.Add(conceptToCheck);
			knowledgeBase.Concepts.Add(wholeConcept);
			knowledgeBase.Concepts.Add(partConcept);
			knowledgeBase.Statements.Add(new HasPartStatement(wholeConcept, partConcept));

			var questionToCheck = new EnumeratePartsQuestion(conceptToCheck);
			var questionPart = new EnumeratePartsQuestion(partConcept);

			// act
			var answerToCheck = questionToCheck.Ask(knowledgeBase.Context);

			var answerPart = questionPart.Ask(knowledgeBase.Context);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerPart.IsEmpty);
			Assert.AreEqual(0, answerPart.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnAllParts()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var wholeConcept = new Concept();
			knowledgeBase.Concepts.Add(wholeConcept);

			const int partCount = 4;

			for (int i = 1; i <= partCount; i++)
			{
				// act
				var partConcept = new Concept();
				knowledgeBase.Concepts.Add(partConcept);
				knowledgeBase.Statements.Add(new HasPartStatement(wholeConcept, partConcept));

				var question = new EnumeratePartsQuestion(wholeConcept);

				var answer = question.Ask(knowledgeBase.Context);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var partConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, partConcepts.Count);
				Assert.IsTrue(partConcepts.All(knowledgeBase.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(knowledgeBase.Statements.Except(answer.Explanation.Statements).Any());
			}
		}
	}
}
