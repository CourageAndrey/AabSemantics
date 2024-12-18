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
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
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
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
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
			Assert.That(answer.IsEmpty, Is.False);

			Assert.That(answer.Explanation.Statements.Single(), Is.SameAs(groupStatement));

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.That(conceptsAnswer.Result.Single(), Is.EqualTo(concept));

			Assert.That(text.Contains(" subject area contains following concepts:"), Is.True);
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
			Assert.That(answer.IsEmpty, Is.False);

			var conceptsAnswer = (ConceptsAnswer) answer;
			Assert.That(conceptsAnswer.Result.Count, Is.GreaterThan(0));

			var groupStatements = answer.Explanation.Statements.OfType<GroupStatement>().ToList();
			Assert.That(groupStatements.Count, Is.EqualTo(answer.Explanation.Statements.Count));

			Assert.That(groupStatements.All(statement => statement.Area == semanticNetwork.SubjectArea_Transport), Is.True);
			Assert.That(conceptsAnswer.Result.Count, Is.EqualTo(groupStatements.Count));
			Assert.That(groupStatements.Select(statement => statement.Concept).Intersect(conceptsAnswer.Result).Count(), Is.EqualTo(groupStatements.Count));
		}
	}
}
