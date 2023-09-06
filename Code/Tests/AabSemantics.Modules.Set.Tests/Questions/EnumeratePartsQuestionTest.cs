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
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
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
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerPart.IsEmpty);
			Assert.AreEqual(0, answerPart.Explanation.Statements.Count);
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
				Assert.IsFalse(answer.IsEmpty);
				var partConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, partConcepts.Count);
				Assert.IsTrue(partConcepts.All(semanticNetwork.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any());

				Assert.IsTrue(text.Contains(" consists of:"));
			}
		}
	}
}
