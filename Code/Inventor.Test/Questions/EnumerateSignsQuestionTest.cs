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
	public class EnumerateSignsQuestionTest
	{
		[Test]
		public void ReturnEmptyAnswerIfNoSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var questionWithoutRecursion = new EnumerateSignsQuestion(LogicalValues.True, false);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(semanticNetwork.Context);
			var answerWithRecursion = semanticNetwork.Ask().WhichSignsHas(LogicalValues.True);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnOwnSignsOnly()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var ownSign = TestHelper.CreateConcept();
			ownSign.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(ownSign).IsSignOf(semanticNetwork.Vehicle_Motorcycle);

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = (ConceptsAnswer) question.Ask(semanticNetwork.SemanticNetwork.Context);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, true);

			// act
			var answer = (ConceptsAnswer) question.Ask(semanticNetwork.SemanticNetwork.Context);

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
			var semanticNetwork = new TestSemanticNetwork(language);

			var ownSign = TestHelper.CreateConcept();
			ownSign.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(ownSign).IsSignOf(semanticNetwork.Vehicle_Motorcycle);

			// act
			var answer = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichSignsHas(semanticNetwork.Vehicle_Motorcycle);

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
