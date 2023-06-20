using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Modules.Set.Questions;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Questions
{
	[TestFixture]
	public class FindSubjectAreaQuestionTest
	{
		[Test]
		public void ConceptWithoutSubjectAreaHasNoSubjectArea()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);
			var conceptsWithoutSubjectArea = new List<IConcept>(SystemConcepts.GetAll()) { semanticNetwork.SubjectArea_Transport };

			foreach (var concept in conceptsWithoutSubjectArea)
			{
				// act
				var answer = semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(concept);

				// assert
				Assert.IsTrue(answer.IsEmpty);
				Assert.AreEqual(0, answer.Explanation.Statements.Count);
			}
		}

		[Test]
		public void ConceptWithSubjectAreaCanFindIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			IList<IConcept> subjectAreas = new IConcept[]
			{
				semanticNetwork.SubjectArea_Transport,
				semanticNetwork.SubjectArea_Numbers,
				semanticNetwork.SubjectArea_Processes,
			};

			var conceptsWithoutSubjectArea = new List<IConcept>(SystemConcepts.GetAll());
			conceptsWithoutSubjectArea.AddRange(subjectAreas);

			foreach (var concept in semanticNetwork.SemanticNetwork.Concepts.Except(conceptsWithoutSubjectArea))
			{
				// act
				var answer = semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(concept);

				// assert
				Assert.IsFalse(answer.IsEmpty);

				var groupStatement = (GroupStatement) answer.Explanation.Statements.Single();
				Assert.IsTrue(subjectAreas.Contains(groupStatement.Area));
				Assert.AreSame(concept, groupStatement.Concept);

				var conceptsAnswer = (ConceptsAnswer) answer;
				Assert.AreSame(groupStatement.Area, conceptsAnswer.Result.Single());
			}
		}

		[Test]
		public void MultipleSubjectAreas()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);
			var concept = semanticNetwork.Base_Vehicle;

			var secondSubjectArea = Modules.Boolean.Concepts.LogicalValues.True;
			semanticNetwork.SemanticNetwork.DeclareThat(concept).BelongsToSubjectArea(secondSubjectArea);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(concept);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.OfType<GroupStatement>().Any(s => s.Area == semanticNetwork.SubjectArea_Transport && s.Concept == concept));
			Assert.IsTrue(answer.Explanation.Statements.OfType<GroupStatement>().Any(s => s.Area == secondSubjectArea && s.Concept == concept));
			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.IsTrue(conceptsAnswer.Result.Contains(semanticNetwork.SubjectArea_Transport));
			Assert.IsTrue(conceptsAnswer.Result.Contains(secondSubjectArea));
		}
	}
}
