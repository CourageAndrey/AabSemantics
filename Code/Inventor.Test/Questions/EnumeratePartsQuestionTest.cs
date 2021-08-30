using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class EnumeratePartsQuestionTest
	{
		[Test]
		public void WhenNoRelationshipsReturnEmptyAnswer()
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
			var answerToCheck = semanticNetwork.Ask().WhichPartsHas(conceptToCheck);

			var answerPart = semanticNetwork.Ask().WhichPartsHas(partConcept);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerPart.IsEmpty);
			Assert.AreEqual(0, answerPart.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnAllParts()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var wholeConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(wholeConcept);

			const int partCount = 4;

			for (int i = 1; i <= partCount; i++)
			{
				// act
				var partConcept = ConceptCreationHelper.CreateConcept();
				semanticNetwork.Concepts.Add(partConcept);
				semanticNetwork.DeclareThat(partConcept).IsPartOf(wholeConcept);

				var answer = semanticNetwork.Ask().WhichPartsHas(wholeConcept);

				// assert
				Assert.IsFalse(answer.IsEmpty);
				var partConcepts = ((ConceptsAnswer) answer).Result;
				Assert.AreEqual(i, partConcepts.Count);
				Assert.IsTrue(partConcepts.All(semanticNetwork.Concepts.Contains));
				Assert.AreEqual(i, answer.Explanation.Statements.Count);
				Assert.IsFalse(semanticNetwork.Statements.Except(answer.Explanation.Statements).Any());
			}
		}
	}
}
