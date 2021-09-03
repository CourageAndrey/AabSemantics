using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class EnumerateAncestorsQuestionTest
	{
		[Test]
		public void WhenNoRelationshipsReturnEmptyAnswer()
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
		public void ReturnAllParents()
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
			}
		}
	}
}
