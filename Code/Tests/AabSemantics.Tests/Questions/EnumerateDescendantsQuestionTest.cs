﻿using System;
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
	public class EnumerateDescendantsQuestionTest
	{
		static EnumerateDescendantsQuestionTest()
		{
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
		}

		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new EnumerateDescendantsQuestion(null));
		}

		[Test]
		public void GivenWhichDescendantsHas_WhenBeingAsked_ThenBuildAndAskQuestion()
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
			var questionRegular = new EnumerateDescendantsQuestion(vehicle);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.Ask().WhichDescendantsHas(vehicle);

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
			var parentConcept = ConceptCreationHelper.CreateEmptyConcept();
			var childConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(conceptToCheck);
			semanticNetwork.Concepts.Add(parentConcept);
			semanticNetwork.Concepts.Add(childConcept);
			semanticNetwork.DeclareThat(childConcept).IsDescendantOf(parentConcept);

			// act
			var answerToCheck = semanticNetwork.Ask().WhichDescendantsHas(conceptToCheck);

			var answerChild = semanticNetwork.Ask().WhichDescendantsHas(childConcept);

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

			var parentConcept = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.Concepts.Add(parentConcept);

			const int childCount = 4;

			for (int i = 1; i <= childCount; i++)
			{
				// act
				var childConcept = ConceptCreationHelper.CreateEmptyConcept();
				semanticNetwork.Concepts.Add(childConcept);
				semanticNetwork.DeclareThat(childConcept).IsDescendantOf(parentConcept);

				var answer = semanticNetwork.Ask().WhichDescendantsHas(parentConcept);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var childConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, childConcepts.Count);
				Assert.IsTrue(childConcepts.All(semanticNetwork.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any());
				Assert.IsTrue(answer.Description.ToString().Contains("can be following:"));
			}
		}
	}
}
