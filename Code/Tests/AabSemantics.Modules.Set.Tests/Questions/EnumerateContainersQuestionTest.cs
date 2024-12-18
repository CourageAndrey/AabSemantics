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
	public class EnumerateContainersQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new EnumerateContainersQuestion(null));
		}

		[Test]
		public void GivenWhichContainersInclude_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new EnumerateContainersQuestion(semanticNetwork.Part_Engine);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichContainersInclude(semanticNetwork.Part_Engine);

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
			var answerToCheck = semanticNetwork.Ask().WhichContainersInclude(conceptToCheck);

			var answerWhole = semanticNetwork.Ask().WhichContainersInclude(wholeConcept);

			// assert
			Assert.That(answerToCheck.IsEmpty, Is.True);
			Assert.That(answerToCheck.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerWhole.IsEmpty, Is.True);
			Assert.That(answerWhole.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnAllContainers()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>()
				.WithModule<SetModule>();

			var partConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(partConcept);

			var render = TextRenders.PlainString;

			const int containerCount = 4;

			for (int i = 1; i <= containerCount; i++)
			{
				// act
				var wholeConcept = ConceptCreationHelper.CreateEmptyConcept();
				semanticNetwork.Concepts.Add(wholeConcept);
				semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

				var answer = semanticNetwork.Ask().WhichContainersInclude(partConcept);
				var text = render.RenderText(answer.Description, language).ToString();

				// assert
				Assert.That(answer.IsEmpty, Is.False);
				var containerConcepts = ((ConceptsAnswer) answer).Result;
				Assert.That(containerConcepts.Count, Is.EqualTo(i));
				Assert.That(containerConcepts.All(semanticNetwork.Concepts.Contains), Is.True);
				Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(i));
				Assert.That(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any(), Is.False);

				Assert.That(text.Contains(" is part of:"), Is.True);
			}
		}
	}
}
