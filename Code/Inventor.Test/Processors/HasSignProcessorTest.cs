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
	public class HasSignProcessorTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language).KnowledgeBase;

			var questionWithoutRecursion = new HasSignQuestion(LogicalValues.True, LogicalValues.False, false);
			var questionWithRecursion = new HasSignQuestion(LogicalValues.True, LogicalValues.False, true);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(knowledgeBase.Context);
			var answerWithRecursion = questionWithRecursion.Ask(knowledgeBase.Context);

			// assert
			Assert.IsFalse(answerWithoutRecursion.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerWithoutRecursion).Result);
			Assert.AreEqual(0, answerWithoutRecursion.Explanation.Statements.Count);

			Assert.IsFalse(answerWithRecursion.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerWithRecursion).Result);
			Assert.AreEqual(0, answerWithoutRecursion.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnEmptyAnswerIfThereAreParentSignsButNotRecursive()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new HasSignQuestion(knowledgeBase.Vehicle_Motorcycle, knowledgeBase.Sign_AreaType, false);

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

			var questionOwnSign = new HasSignQuestion(knowledgeBase.Vehicle_Motorcycle, ownSign, false);
			var questionInheritedSign = new HasSignQuestion(knowledgeBase.Vehicle_Motorcycle, knowledgeBase.Sign_AreaType, false);

			// act
			var answerOwnSign = questionOwnSign.Ask(knowledgeBase.KnowledgeBase.Context);
			var answerInheritedSign = questionInheritedSign.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answerOwnSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerOwnSign).Result);
			Assert.AreSame(ownSignStatement, answerOwnSign.Explanation.Statements.Single());

			Assert.IsFalse(answerInheritedSign.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerInheritedSign).Result);
			Assert.AreEqual(0, answerInheritedSign.Explanation.Statements.Count);
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

			var questionOwnSign = new HasSignQuestion(knowledgeBase.Vehicle_Motorcycle, ownSign, true);
			var questionInheritedSign = new HasSignQuestion(knowledgeBase.Vehicle_Motorcycle, knowledgeBase.Sign_AreaType, true);

			// act
			var answerOwnSign = questionOwnSign.Ask(knowledgeBase.KnowledgeBase.Context);
			var answerInheritedSign = questionInheritedSign.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answerOwnSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerOwnSign).Result);
			Assert.AreSame(ownSignStatement, answerOwnSign.Explanation.Statements.Single());

			Assert.IsFalse(answerInheritedSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerInheritedSign).Result);
			Assert.AreEqual(2, answerInheritedSign.Explanation.Statements.Count);
			Assert.AreEqual(1, answerInheritedSign.Explanation.Statements.OfType<HasSignStatement>().Count());
			Assert.AreEqual(1, answerInheritedSign.Explanation.Statements.OfType<IsStatement>().Count());
		}
	}
}
