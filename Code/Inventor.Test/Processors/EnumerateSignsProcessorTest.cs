using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Processors
{
	[TestFixture]
	public class EnumerateSignsProcessorTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language).KnowledgeBase;

			var questionWithoutRecursion = new EnumerateSignsQuestion(LogicalValues.True, false);
			var questionWithRecursion = new EnumerateSignsQuestion(LogicalValues.True, true);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(knowledgeBase.Context);
			var answerWithRecursion = questionWithRecursion.Ask(knowledgeBase.Context);

			// assert
			Assert.IsTrue(answerWithoutRecursion.IsEmpty);
			Assert.AreEqual(0, answerWithoutRecursion.Explanation.Statements.Count);

			Assert.IsTrue(answerWithRecursion.IsEmpty);
			Assert.AreEqual(0, answerWithRecursion.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnEmptyAnswerIfThereAreParentSignsButNotRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new EnumerateSignsQuestion(knowledgeBase.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnOwnSignsOnly()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			knowledgeBase.KnowledgeBase.Concepts.Add(ownSign);

			var ownSignStatement = new HasSignStatement(knowledgeBase.Vehicle_Motorcycle, ownSign);
			knowledgeBase.KnowledgeBase.Statements.Add(ownSignStatement);

			var question = new EnumerateSignsQuestion(knowledgeBase.Vehicle_Motorcycle, false);

			// act
			var answer = (ConceptsAnswer) question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(ownSign, answer.Result.Single());
			Assert.AreSame(ownSignStatement, answer.Explanation.Statements.Single());
		}

		[Test]
		public void ReturnParentOnlySignsIfRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new EnumerateSignsQuestion(knowledgeBase.Vehicle_Motorcycle, true);

			// act
			var answer = (ConceptsAnswer) question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreEqual(2, answer.Result.Count);
			Assert.AreEqual(1, answer.Explanation.Statements.OfType<IsStatement>().Count());
			Assert.AreEqual(2, answer.Explanation.Statements.OfType<HasSignStatement>().Count());
		}

		[Test]
		public void ReturnParentAndOwnSignsIfRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			knowledgeBase.KnowledgeBase.Concepts.Add(ownSign);

			var ownSignStatement = new HasSignStatement(knowledgeBase.Vehicle_Motorcycle, ownSign);
			knowledgeBase.KnowledgeBase.Statements.Add(ownSignStatement);

			var question = new EnumerateSignsQuestion(knowledgeBase.Vehicle_Motorcycle, true);

			// act
			var answer = (ConceptsAnswer) question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreEqual(3, answer.Result.Count);
			Assert.IsTrue(answer.Result.Contains(ownSign));
			Assert.IsTrue(answer.Explanation.Statements.Contains(ownSignStatement));
			Assert.AreEqual(1, answer.Explanation.Statements.OfType<IsStatement>().Count());
			Assert.AreEqual(3, answer.Explanation.Statements.OfType<HasSignStatement>().Count());
		}
	}
}
