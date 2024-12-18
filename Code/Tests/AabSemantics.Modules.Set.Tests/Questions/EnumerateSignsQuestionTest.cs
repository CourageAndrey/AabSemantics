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
	public class EnumerateSignsQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new EnumerateSignsQuestion(null, false));
		}

		[Test]
		public void GivenWhichSignsHas_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Car, true);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichSignsHas(semanticNetwork.Vehicle_Car);

			// assert
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenNoSigns_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData().SemanticNetwork;

			var questionWithoutRecursion = new EnumerateSignsQuestion(LogicalValues.True, false);

			// act
			var answerWithoutRecursion = questionWithoutRecursion.Ask(semanticNetwork.Context);
			var answerWithRecursion = semanticNetwork.Ask().WhichSignsHas(LogicalValues.True);

			// assert
			Assert.That(answerWithoutRecursion.IsEmpty, Is.True);
			Assert.That(answerWithoutRecursion.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerWithRecursion.IsEmpty, Is.True);
			Assert.That(answerWithRecursion.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenParentSignsButNotRecursive_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.True);
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

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(ownSign).IsSignOf(semanticNetwork.Vehicle_Motorcycle);

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, false);

			// act
			var answer = (ConceptsAnswer) question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Result.Single(), Is.SameAs(ownSign));
			Assert.That(answer.Explanation.Statements.Single(), Is.SameAs(ownSignStatement));
		}

		[Test]
		public void GivenRecursive_WhenBeingAsked_ThenReturnParentSignsOnly()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var question = new EnumerateSignsQuestion(semanticNetwork.Vehicle_Motorcycle, true);

			// act
			var answer = (ConceptsAnswer) question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Result.Count, Is.EqualTo(2));
			Assert.That(answer.Explanation.Statements.OfType<IsStatement>().Count(), Is.EqualTo(1));
			Assert.That(answer.Explanation.Statements.OfType<HasSignStatement>().Count(), Is.EqualTo(2));
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

			var ownSignStatement = semanticNetwork.SemanticNetwork.DeclareThat(ownSign).IsSignOf(semanticNetwork.Vehicle_Motorcycle);

			var render = TextRenders.PlainString;

			// act
			var answer = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichSignsHas(semanticNetwork.Vehicle_Motorcycle);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(answer.Result.Count, Is.EqualTo(3));
			Assert.That(answer.Result.Contains(ownSign), Is.True);
			Assert.That(answer.Explanation.Statements.Contains(ownSignStatement), Is.True);
			Assert.That(answer.Explanation.Statements.OfType<IsStatement>().Count(), Is.EqualTo(1));
			Assert.That(answer.Explanation.Statements.OfType<HasSignStatement>().Count(), Is.EqualTo(3));

			Assert.That(text.Contains(" concept has following signs"), Is.True);
		}
	}
}
