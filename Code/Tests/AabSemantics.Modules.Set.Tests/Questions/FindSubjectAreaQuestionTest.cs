using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class FindSubjectAreaQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new FindSubjectAreaQuestion(null));
		}

		[Test]
		public void GivenToWhichSubjectAreasBelongs_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new FindSubjectAreaQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(semanticNetwork.Vehicle_Car);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();
			var conceptsWithoutSubjectArea = new List<IConcept>(LogicalValues.All) { semanticNetwork.SubjectArea_Transport };

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
		public void GivenSingleSubjectArea_WhenBeingAsked_ThenReturnIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			IList<IConcept> subjectAreas = new IConcept[]
			{
				semanticNetwork.SubjectArea_Transport,
			};

			var conceptsWithoutSubjectArea = new List<IConcept>(LogicalValues.All);
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
		public void GivenMultipleSubjectAreas_WhenBeingAsked_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();
			var concept = semanticNetwork.Base_Vehicle;

			var secondSubjectArea = LogicalValues.True;
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
