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
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConcept());
			semanticNetwork.Concepts.Add(car = "car".CreateConcept());
			semanticNetwork.DeclareThat(car).IsDescendantOf(vehicle);

			// act
			var questionRegular = new EnumerateAncestorsQuestion(car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.Ask().WhichAncestorsHas(car);

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

			var conceptToCheck = ConceptCreationHelper.CreateConcept();
			var parentConcept = ConceptCreationHelper.CreateConcept();
			var childConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(conceptToCheck);
			semanticNetwork.Concepts.Add(parentConcept);
			semanticNetwork.Concepts.Add(childConcept);
			semanticNetwork.DeclareThat(childConcept).IsDescendantOf(parentConcept);

			// act
			var answerToCheck = semanticNetwork.Ask().WhichAncestorsHas(conceptToCheck);

			var answerChild = semanticNetwork.Ask().WhichAncestorsHas(parentConcept);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerChild.IsEmpty);
			Assert.AreEqual(0, answerChild.Explanation.Statements.Count);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnAllRelated()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var childConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(childConcept);

			const int parentCount = 4;

			for (int i = 1; i <= parentCount; i++)
			{
				// act
				var parentConcept = ConceptCreationHelper.CreateConcept();
				semanticNetwork.Concepts.Add(parentConcept);
				semanticNetwork.DeclareThat(parentConcept).IsAncestorOf(childConcept);

				var answer = semanticNetwork.Ask().WhichAncestorsHas(childConcept);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var parentConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, parentConcepts.Count);
				Assert.IsTrue(parentConcepts.All(semanticNetwork.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any());
				Assert.IsTrue(answer.Description.ToString().Contains("is:"));
			}
		}
	}
}
