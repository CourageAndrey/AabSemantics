using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class HasSignsQuestionTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var questionWithoutRecursion = new HasSignsQuestion(LogicalValues.True, false);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(semanticNetwork.Context);
			var answerWithRecursion = semanticNetwork.Ask().IfHasSigns(LogicalValues.True);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var question = new HasSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var ownSign = ConceptCreationHelper.CreateConcept();
			ownSign.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			var questionOwnSign = new HasSignsQuestion(semanticNetwork.Base_Vehicle, false);
			var questionInheritedSign = new HasSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answerOwnSign = questionOwnSign.Ask(semanticNetwork.SemanticNetwork.Context);
			var answerInheritedSign = questionInheritedSign.Ask(semanticNetwork.SemanticNetwork.Context);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var ownSign = ConceptCreationHelper.CreateConcept();
			ownSign.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			// act
			var answerOwnSign = semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Vehicle_Motorcycle);
			var answerInheritedSign = semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Base_Vehicle);

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
