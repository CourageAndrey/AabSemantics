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
	public class HasSignQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new HasSignQuestion(null, concept, false));
			Assert.Throws<ArgumentNullException>(() => new HasSignQuestion(concept, null, false));
		}

		[Test]
		public void GivenIfHasSign_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new HasSignQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType, true);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);

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
		public void GivenParentSignsButNotRecursive_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var question = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType, false);

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

			var questionOwnSign = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, ownSign, false);
			var questionInheritedSign = new HasSignQuestion(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType, false);

			var render = TextRenders.PlainString;

			// act
			var answerOwnSign = questionOwnSign.Ask(semanticNetwork.SemanticNetwork.Context);
			var textOwnSign = render.RenderText(answerOwnSign.Description, language).ToString();
			var answerInheritedSign = questionInheritedSign.Ask(semanticNetwork.SemanticNetwork.Context);
			var textInheritedSign = render.RenderText(answerInheritedSign.Description, language).ToString();

			// assert
			Assert.IsFalse(answerOwnSign.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answerOwnSign).Result);
			Assert.AreSame(ownSignStatement, answerOwnSign.Explanation.Statements.Single());

			Assert.IsFalse(answerInheritedSign.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answerInheritedSign).Result);
			Assert.AreEqual(0, answerInheritedSign.Explanation.Statements.Count);

			Assert.IsTrue(textOwnSign.Contains(" has got "));
			Assert.IsTrue(textInheritedSign.Contains(" has not got "));
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
