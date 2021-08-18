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
	public class HasSignQuestionTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var questionWithoutRecursion = new HasSignQuestion(LogicalValues.True, LogicalValues.False, false);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(semanticNetwork.Context);
			var answerWithRecursion = semanticNetwork.Ask().IfHasSign(LogicalValues.True, LogicalValues.False);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var question = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType, false);

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

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			var questionOwnSign = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, ownSign, false);
			var questionInheritedSign = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType, false);

			// act
			var answerOwnSign = questionOwnSign.Ask(semanticNetwork.SemanticNetwork.Context);
			var answerInheritedSign = questionInheritedSign.Ask(semanticNetwork.SemanticNetwork.Context);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var ownSign = new Concept();
			ownSign.Attributes.Add(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			// act
			var answerOwnSign = semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Motorcycle, ownSign);
			var answerInheritedSign = semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType);

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
