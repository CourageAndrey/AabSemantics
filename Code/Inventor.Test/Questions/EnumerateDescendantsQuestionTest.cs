using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class EnumerateDescendantsQuestionTest
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
			var answerToCheck = semanticNetwork.Ask().WhichDescendantsHas(conceptToCheck);

			var answerChild = semanticNetwork.Ask().WhichDescendantsHas(childConcept);

			// assert
			Assert.IsTrue(answerToCheck.IsEmpty);
			Assert.AreEqual(0, answerToCheck.Explanation.Statements.Count);

			Assert.IsTrue(answerChild.IsEmpty);
			Assert.AreEqual(0, answerChild.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnAllChildren()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var parentConcept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(parentConcept);

			const int childCount = 4;

			for (int i = 1; i <= childCount; i++)
			{
				// act
				var childConcept = ConceptCreationHelper.CreateConcept();
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
			}
		}
	}
}
