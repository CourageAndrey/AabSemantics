using System;
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
	public class DescribeSubjectAreaQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new DescribeSubjectAreaQuestion(null));
		}

		[Test]
		public void GivenWhichConceptsBelongToSubjectArea_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new DescribeSubjectAreaQuestion(semanticNetwork.SubjectArea_Transport);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(semanticNetwork.SubjectArea_Transport);

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
			var noSubjectAreaConcept = LogicalValues.True;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(noSubjectAreaConcept);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenSingleConcept_WhenBeingAsked_ThenReturnIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var area = LogicalValues.True;
			var concept = LogicalValues.False;
			var groupStatement = semanticNetwork.SemanticNetwork.DeclareThat(concept).BelongsToSubjectArea(area);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(area);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsFalse(answer.IsEmpty);

			Assert.AreSame(groupStatement, answer.Explanation.Statements.Single());

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.AreEqual(concept, conceptsAnswer.Result.Single());

			Assert.IsTrue(text.Contains(" subject area contains following concepts:"));
		}

		[Test]
		public void GivenMultipleConcepts_WhenBeingAsked_ThenReturnThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhichConceptsBelongToSubjectArea(semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.Greater(conceptsAnswer.Result.Count, 0);

			var groupStatements = answer.Explanation.Statements.OfType<GroupStatement>().ToList();
			Assert.AreEqual(answer.Explanation.Statements.Count, groupStatements.Count);

			Assert.IsTrue(groupStatements.All(statement => statement.Area == semanticNetwork.SubjectArea_Transport));
			Assert.AreEqual(groupStatements.Count, conceptsAnswer.Result.Count);
			Assert.AreEqual(groupStatements.Count, groupStatements.Select(statement => statement.Concept).Intersect(conceptsAnswer.Result).Count());
		}
	}
}
