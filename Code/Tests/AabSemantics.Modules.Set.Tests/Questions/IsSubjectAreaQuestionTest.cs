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
			IConcept concept = "test".CreateConcept();

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
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, LogicalValues.True);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenCorrespondingInformation_WhenBeingAsked_ThenReturnTrue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfConceptBelongsToSubjectArea(semanticNetwork.Base_Vehicle, semanticNetwork.SubjectArea_Transport);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (GroupStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Concept == semanticNetwork.Base_Vehicle && statement.Area == semanticNetwork.SubjectArea_Transport);
		}
	}
}
