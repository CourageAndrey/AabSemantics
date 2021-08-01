using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
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
				var question = new FindSubjectAreaQuestion(concept);
				var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

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
			};

			var conceptsWithoutSubjectArea = new List<IConcept>(SystemConcepts.GetAll());
			conceptsWithoutSubjectArea.AddRange(subjectAreas);

			foreach (var concept in semanticNetwork.SemanticNetwork.Concepts.Except(conceptsWithoutSubjectArea))
			{
				// act
				var question = new FindSubjectAreaQuestion(concept);
				var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

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

			var secondSubjectArea = LogicalValues.True;
			semanticNetwork.SemanticNetwork.Statements.Add(new GroupStatement(secondSubjectArea, concept));

			// act
			var question = new FindSubjectAreaQuestion(concept);
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

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
