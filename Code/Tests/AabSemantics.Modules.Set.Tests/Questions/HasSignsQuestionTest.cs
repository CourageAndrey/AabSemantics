using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class HasSignsQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignsQuestion(null, false));
		}

		[Test]
		public void GivenIfHasSigns_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new HasSignsQuestion(semanticNetwork.Vehicle_Car, true);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Vehicle_Car);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoSigns_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData().SemanticNetwork;

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
		public void GivenParentSignsButNotRecursive_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var question = new HasSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenNoRecursion_WhenBeingAsked_ThenReturnOwnSignsOnly()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

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
		public void GivenRecursive_WhenBeingAsked_ThenReturnAllSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

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
