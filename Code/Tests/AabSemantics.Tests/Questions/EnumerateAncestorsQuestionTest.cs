using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class EnumerateAncestorsQuestionTest
	{
		static EnumerateAncestorsQuestionTest()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
		}

		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new EnumerateAncestorsQuestion(null));
		}

		[Test]
		public void GivenWhichAncestorsHas_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, car;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(car = "car".CreateConceptByName());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var questionRegular = new EnumerateAncestorsQuestion(car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.Ask().WhichAncestorsHas(car);

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
			var parentConcept = ConceptCreationHelper.CreateEmptyConcept();
			var childConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(conceptToCheck);
			semanticNetwork.Concepts.Add(parentConcept);
			semanticNetwork.Concepts.Add(childConcept);
			semanticNetwork.DeclareThat(childConcept).IsDescendantOf(parentConcept);

			// act
			var answerToCheck = semanticNetwork.Ask().WhichAncestorsHas(conceptToCheck);

			var answerChild = semanticNetwork.Ask().WhichAncestorsHas(parentConcept);

			// assert
			Assert.That(answerToCheck.IsEmpty, Is.True);
			Assert.That(answerToCheck.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(answerChild.IsEmpty, Is.True);
			Assert.That(answerChild.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnAllRelated()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var childConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(childConcept);

			const int parentCount = 4;

			for (int i = 1; i <= parentCount; i++)
			{
				// act
				var parentConcept = ConceptCreationHelper.CreateEmptyConcept();
				semanticNetwork.Concepts.Add(parentConcept);
				semanticNetwork.DeclareThat(parentConcept).IsAncestorOf(childConcept);

				var answer = semanticNetwork.Ask().WhichAncestorsHas(childConcept);

				// assert
				Assert.That(answer.IsEmpty, Is.False);
				var parentConcepts = ((ConceptsAnswer) answer).Result;
				Assert.That(parentConcepts.Count, Is.EqualTo(i));
				Assert.That(parentConcepts.All(semanticNetwork.Concepts.Contains), Is.True);
				Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(i));
				Assert.That(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any(), Is.False);
				Assert.That(answer.Description.ToString().Contains("is:"), Is.True);
			}
		}
	}
}
