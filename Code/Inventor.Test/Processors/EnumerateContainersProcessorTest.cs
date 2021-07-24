﻿using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Processors
{
	[TestFixture]
	public class EnumerateContainersProcessorTest
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

			var questionToCheck = new EnumerateContainersQuestion(conceptToCheck);
			var questionWhole = new EnumerateContainersQuestion(wholeConcept);

			// act
			var answerToCheck = questionToCheck.Ask(knowledgeBase.Context);

			var answerWhole = questionWhole.Ask(knowledgeBase.Context);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerWhole.IsEmpty);
			Assert.AreEqual(0, answerWhole.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnAllContainers()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var partConcept = new Concept();
			knowledgeBase.Concepts.Add(partConcept);

			const int containerCount = 4;

			for (int i = 1; i <= containerCount; i++)
			{
				// act
				var wholeConcept = new Concept();
				knowledgeBase.Concepts.Add(wholeConcept);
				knowledgeBase.Statements.Add(new HasPartStatement(wholeConcept, partConcept));

				var question = new EnumerateContainersQuestion(partConcept);

				var answer = question.Ask(knowledgeBase.Context);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var containerConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, containerConcepts.Count);
				Assert.IsTrue(containerConcepts.All(knowledgeBase.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(knowledgeBase.Statements.Except(answer.Explanation.Statements).Any());
			}
		}
	}
}