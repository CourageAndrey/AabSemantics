using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class EnumerateContainersQuestionTest
	{
		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var conceptToCheck = ConceptCreationHelper.CreateConcept();
			var wholeConcept = ConceptCreationHelper.CreateConcept();
			var partConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(conceptToCheck);
			semanticNetwork.Concepts.Add(wholeConcept);
			semanticNetwork.Concepts.Add(partConcept);
			semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

			// act
			var answerToCheck = semanticNetwork.Ask().WhichContainersInclude(conceptToCheck);

			var answerWhole = semanticNetwork.Ask().WhichContainersInclude(wholeConcept);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerWhole.IsEmpty);
			Assert.AreEqual(0, answerWhole.Explanation.Statements.Count);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnAllContainers()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var partConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(partConcept);

			const int containerCount = 4;

			for (int i = 1; i <= containerCount; i++)
			{
				// act
				var wholeConcept = ConceptCreationHelper.CreateConcept();
				semanticNetwork.Concepts.Add(wholeConcept);
				semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

				var answer = semanticNetwork.Ask().WhichContainersInclude(partConcept);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var containerConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, containerConcepts.Count);
				Assert.IsTrue(containerConcepts.All(semanticNetwork.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any());
			}
		}
	}
}
