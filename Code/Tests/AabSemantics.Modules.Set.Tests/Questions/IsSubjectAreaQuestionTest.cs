using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class IsSubjectAreaQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsSubjectAreaQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new IsSubjectAreaQuestion(concept, null));
		}

		[Test]
		public void GivenIfConceptBelongsToSubjectArea_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new IsSubjectAreaQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.SubjectArea_Transport);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Vehicle_Car, semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, LogicalValues.True);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(text.Contains(" concept does not belong to "), Is.True);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnTrue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, semanticNetwork.SubjectArea_Transport);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.True);

			var statement = (GroupStatement) answer.Explanation.Statements.Single();
			Assert.That(statement.Concept == semanticNetwork.Base_Vehicle && statement.Area == semanticNetwork.SubjectArea_Transport, Is.True);

			Assert.That(text.Contains(" concept belongs to "), Is.True);
		}
	}
}
