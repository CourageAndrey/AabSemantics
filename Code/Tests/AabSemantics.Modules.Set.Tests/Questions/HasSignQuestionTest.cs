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
			IConcept concept = "test".CreateConceptByName();

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
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
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
			Assert.That(answerWithoutRecursion.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerWithoutRecursion).Result, Is.False);
			Assert.That(answerWithoutRecursion.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerWithRecursion.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerWithRecursion).Result, Is.False);
			Assert.That(answerWithRecursion.Explanation.Statements.Count, Is.EqualTo(0));
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
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenNoRecursion_WhenBeingAsked_ThenReturnOwnSignsOnly()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var ownSign = ConceptCreationHelper.CreateEmptyConcept();
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
			Assert.That(answerOwnSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerOwnSign).Result, Is.True);
			Assert.That(answerOwnSign.Explanation.Statements.Single(), Is.SameAs(ownSignStatement));

			Assert.That(answerInheritedSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerInheritedSign).Result, Is.False);
			Assert.That(answerInheritedSign.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(textOwnSign.Contains(" has got "), Is.True);
			Assert.That(textInheritedSign.Contains(" has not got "), Is.True);
		}

		[Test]
		public void GivenRecursive_WhenBeingAsked_ThenReturnAllSigns()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var ownSign = ConceptCreationHelper.CreateEmptyConcept();
			ownSign.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.SemanticNetwork.Concepts.Add(ownSign);

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			// act
			var answerOwnSign = semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Motorcycle, ownSign);
			var answerInheritedSign = semanticNetwork.SemanticNetwork.Ask().IfHasSign(semanticNetwork.Vehicle_Motorcycle, semanticNetwork.Sign_AreaType);

			// assert
			Assert.That(answerOwnSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerOwnSign).Result, Is.True);
			Assert.That(answerOwnSign.Explanation.Statements.Single(), Is.SameAs(ownSignStatement));

			Assert.That(answerInheritedSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerInheritedSign).Result, Is.True);
			Assert.That(answerInheritedSign.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answerInheritedSign.Explanation.Statements.OfType<HasSignStatement>().Count(), Is.EqualTo(1));
			Assert.That(answerInheritedSign.Explanation.Statements.OfType<IsStatement>().Count(), Is.EqualTo(1));
		}
	}
}
