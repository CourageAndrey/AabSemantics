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
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
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
				Assert.That(answer.IsEmpty, Is.True);
				Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
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
				Assert.That(answer.IsEmpty, Is.False);

				var groupStatement = (GroupStatement) answer.Explanation.Statements.Single();
				Assert.That(subjectAreas.Contains(groupStatement.Area), Is.True);
				Assert.That(groupStatement.Concept, Is.SameAs(concept));

				var conceptsAnswer = (ConceptsAnswer) answer;
				Assert.That(conceptsAnswer.Result.Single(), Is.SameAs(groupStatement.Area));
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

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().ToWhichSubjectAreasBelongs(concept);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);

			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(2));
			Assert.That(answer.Explanation.Statements.OfType<GroupStatement>().Any(s => s.Area == semanticNetwork.SubjectArea_Transport && s.Concept == concept), Is.True);
			Assert.That(answer.Explanation.Statements.OfType<GroupStatement>().Any(s => s.Area == secondSubjectArea && s.Concept == concept), Is.True);
			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.That(conceptsAnswer.Result.Contains(semanticNetwork.SubjectArea_Transport), Is.True);
			Assert.That(conceptsAnswer.Result.Contains(secondSubjectArea), Is.True);

			Assert.That(text.Contains(" belongs to following subject areas:"), Is.True);
		}
	}
}
