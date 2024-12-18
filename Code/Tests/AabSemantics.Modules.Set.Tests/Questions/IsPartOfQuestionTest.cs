using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class IsPartOfQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsPartOfQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new IsPartOfQuestion(concept, null));
		}

		[Test]
		public void GivenIfIsPartOf_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new IsPartOfQuestion(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);

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
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Base_Vehicle);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.False);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(text.Contains(" is not part of "), Is.True);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnTrue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsPartOf(semanticNetwork.Part_Engine, semanticNetwork.Vehicle_Car);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.False);
			Assert.That(((BooleanAnswer) answer).Result, Is.True);

			var statement = (HasPartStatement) answer.Explanation.Statements.Single();
			Assert.That(statement.Whole == semanticNetwork.Vehicle_Car && statement.Part == semanticNetwork.Part_Engine, Is.True);

			Assert.That(text.Contains(" is part of "), Is.True);
		}
	}
}
