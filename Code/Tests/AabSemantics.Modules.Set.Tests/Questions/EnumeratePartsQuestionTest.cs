using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class EnumeratePartsQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new EnumeratePartsQuestion(null));
		}

		[Test]
		public void GivenWhichPartsHas_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new EnumeratePartsQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichPartsHas(semanticNetwork.Vehicle_Car);

			// assert
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var conceptToCheck = ConceptCreationHelper.CreateEmptyConcept();
			var wholeConcept = ConceptCreationHelper.CreateEmptyConcept();
			var partConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(conceptToCheck);
			semanticNetwork.Concepts.Add(wholeConcept);
			semanticNetwork.Concepts.Add(partConcept);
			semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

			// act
			var answerToCheck = semanticNetwork.Ask().WhichPartsHas(conceptToCheck);

			var answerPart = semanticNetwork.Ask().WhichPartsHas(partConcept);

			// assert
			Assert.That(answerToCheck.IsEmpty, Is.True);
			Assert.That(answerToCheck.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerPart.IsEmpty, Is.True);
			Assert.That(answerPart.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnAllParts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<SetModule>();

			var wholeConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(wholeConcept);

			var render = TextRenders.PlainString;

			const int partCount = 4;

			for (int i = 1; i <= partCount; i++)
			{
				// act
				var partConcept = ConceptCreationHelper.CreateEmptyConcept();
				semanticNetwork.Concepts.Add(partConcept);
				semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

				var answer = semanticNetwork.Ask().WhichPartsHas(wholeConcept);
				var text = render.RenderText(answer.Description, language).ToString();

				// assert
				Assert.That(answer.IsEmpty, Is.False);
				var partConcepts = ((ConceptsAnswer) answer).Result;
				Assert.That(partConcepts.Count, Is.EqualTo(i));
				Assert.That(partConcepts.All(semanticNetwork.Concepts.Contains), Is.True);
				Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(i));
				Assert.That(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any(), Is.False);

				Assert.That(text.Contains(" consists of:"), Is.True);
			}
		}
	}
}
