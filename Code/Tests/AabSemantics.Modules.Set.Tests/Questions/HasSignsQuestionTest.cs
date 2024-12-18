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
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
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

			var question = new HasSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			var render = TextRenders.PlainString;

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(text.Contains("has not signs "), Is.True);
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

			var questionOwnSign = new HasSignsQuestion(semanticNetwork.Base_Vehicle, false);
			var questionInheritedSign = new HasSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answerOwnSign = questionOwnSign.Ask(semanticNetwork.SemanticNetwork.Context);
			var answerInheritedSign = questionInheritedSign.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.That(answerOwnSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerOwnSign).Result, Is.True);
			Assert.That(answerOwnSign.Explanation.Statements.Count, Is.EqualTo(2));

			Assert.That(answerInheritedSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerInheritedSign).Result, Is.True);
			Assert.That(answerInheritedSign.Explanation.Statements.Single(), Is.SameAs(ownSignStatement));
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

			semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.Vehicle_Motorcycle).HasSign(ownSign);

			var render = TextRenders.PlainString;

			// act
			var answerOwnSign = semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Vehicle_Motorcycle);
			var textOwnSign = render.RenderText(answerOwnSign.Description, language).ToString();
			var answerInheritedSign = semanticNetwork.SemanticNetwork.Ask().IfHasSigns(semanticNetwork.Base_Vehicle);
			var textInheritedSign = render.RenderText(answerInheritedSign.Description, language).ToString();

			// assert
			Assert.That(answerOwnSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerOwnSign).Result, Is.True);
			Assert.That(answerOwnSign.Explanation.Statements.Count, Is.EqualTo(4));
			Assert.That(answerOwnSign.Explanation.Statements.OfType<HasSignStatement>().Count(), Is.EqualTo(3));
			Assert.That(answerOwnSign.Explanation.Statements.OfType<IsStatement>().Count(), Is.EqualTo(1));

			Assert.That(answerInheritedSign.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answerInheritedSign).Result, Is.True);
			Assert.That(answerInheritedSign.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answerInheritedSign.Explanation.Statements.OfType<HasSignStatement>().Count(), Is.EqualTo(2));

			Assert.That(textOwnSign.Contains("has signs "), Is.True);
			Assert.That(textInheritedSign.Contains("has signs "), Is.True);
		}
	}
}
