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
	public class EnumerateChildrenQuestionTest
	{
		[Test]
		public void WhenNoRelationshipsReturnEmptyAnswer()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var conceptToCheck = new Concept();
			var parentConcept = new Concept();
			var childConcept = new Concept();
			knowledgeBase.Concepts.Add(conceptToCheck);
			knowledgeBase.Concepts.Add(parentConcept);
			knowledgeBase.Concepts.Add(childConcept);
			knowledgeBase.Statements.Add(new IsStatement(parentConcept, childConcept));

			var questionToCheck = new EnumerateChildrenQuestion(conceptToCheck);
			var questionChild = new EnumerateChildrenQuestion(childConcept);

			// act
			var answerToCheck = questionToCheck.Ask(knowledgeBase.Context);

			var answerChild = questionChild.Ask(knowledgeBase.Context);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerChild.IsEmpty);
			Assert.AreEqual(0, answerChild.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnAllChildren()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var parentConcept = new Concept();
			knowledgeBase.Concepts.Add(parentConcept);

			const int childCount = 4;

			for (int i = 1; i <= childCount; i++)
			{
				// act
				var childConcept = new Concept();
				knowledgeBase.Concepts.Add(childConcept);
				knowledgeBase.Statements.Add(new IsStatement(parentConcept, childConcept));

				var question = new EnumerateChildrenQuestion(parentConcept);

				var answer = question.Ask(knowledgeBase.Context);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var childConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, childConcepts.Count);
				Assert.IsTrue(childConcepts.All(knowledgeBase.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(knowledgeBase.Statements.Except(answer.Explanation.Statements).Any());
			}
		}
	}
}
