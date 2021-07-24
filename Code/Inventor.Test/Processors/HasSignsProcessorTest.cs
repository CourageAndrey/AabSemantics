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
	public class HasSignsProcessorTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language).KnowledgeBase;

			var questionWithoutRecursion = new HasSignsQuestion(LogicalValues.True, false);
			var questionWithRecursion = new HasSignsQuestion(LogicalValues.True, true);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(knowledgeBase.Context);
			var answerWithRecursion = questionWithRecursion.Ask(knowledgeBase.Context);

			// assert
			Assert.IsFalse(answerWithoutRecursion.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerWithoutRecursion).Result);
			Assert.AreEqual(0, answerWithoutRecursion.Explanation.Statements.Count);

			Assert.IsFalse(answerWithRecursion.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerWithRecursion).Result);
			Assert.AreEqual(0, answerWithRecursion.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnEmptyAnswerIfThereAreParentSignsButNotRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new HasSignsQuestion(knowledgeBase.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnOwnSignsOnlyIfNoRecursion()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			knowledgeBase.KnowledgeBase.Concepts.Add(ownSign);

			var ownSignStatement = new HasSignStatement(knowledgeBase.Vehicle_Motorcycle, ownSign);
			knowledgeBase.KnowledgeBase.Statements.Add(ownSignStatement);

			var questionOwnSign = new HasSignsQuestion(knowledgeBase.Base_Vehicle, false);
			var questionInheritedSign = new HasSignsQuestion(knowledgeBase.Vehicle_Motorcycle, false);

			// act
			var answerOwnSign = questionOwnSign.Ask(knowledgeBase.KnowledgeBase.Context);
			var answerInheritedSign = questionInheritedSign.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answerOwnSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerOwnSign).Result);
			Assert.AreEqual(2, answerOwnSign.Explanation.Statements.Count);

			Assert.IsFalse(answerInheritedSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerInheritedSign).Result);
			Assert.AreSame(ownSignStatement, answerInheritedSign.Explanation.Statements.Single());
		}

		[Test]
		public void ReturnAllSignsIfRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			knowledgeBase.KnowledgeBase.Concepts.Add(ownSign);

			var ownSignStatement = new HasSignStatement(knowledgeBase.Vehicle_Motorcycle, ownSign);
			knowledgeBase.KnowledgeBase.Statements.Add(ownSignStatement);

			var questionOwnSign = new HasSignsQuestion(knowledgeBase.Vehicle_Motorcycle, true);
			var questionInheritedSign = new HasSignsQuestion(knowledgeBase.Base_Vehicle, true);

			// act
			var answerOwnSign = questionOwnSign.Ask(knowledgeBase.KnowledgeBase.Context);
			var answerInheritedSign = questionInheritedSign.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answerOwnSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerOwnSign).Result);
			Assert.AreEqual(4, answerOwnSign.Explanation.Statements.Count);
			Assert.AreEqual(3, answerOwnSign.Explanation.Statements.OfType<HasSignStatement>().Count());
			Assert.AreEqual(1, answerOwnSign.Explanation.Statements.OfType<IsStatement>().Count());

			Assert.IsFalse(answerInheritedSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerInheritedSign).Result);
			Assert.AreEqual(2, answerInheritedSign.Explanation.Statements.Count);
			Assert.AreEqual(2, answerInheritedSign.Explanation.Statements.OfType<HasSignStatement>().Count());
		}
	}
}
