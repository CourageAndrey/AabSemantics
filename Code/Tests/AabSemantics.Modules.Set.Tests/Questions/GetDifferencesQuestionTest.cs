using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class GetDifferencesQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(concept, null));
		}

		[Test]
		public void GivenSameArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<ArgumentException>(() => new GetDifferencesQuestion(concept, concept));
		}

		[Test]
		public void GivenGetDifferences_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new GetDifferencesQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Airbus);
			var answerRegular = (ConceptsAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptsAnswer) semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Airbus);

			// assert
			Assert.AreSame(semanticNetwork.Vehicle_Car, questionRegular.Concept1);
			Assert.AreSame(semanticNetwork.Vehicle_Airbus, questionRegular.Concept2);
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenConceptsCanNotBeCompared_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.AreaType_Air, semanticNetwork.MotorType_Jet);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);

			Assert.IsTrue(text.Contains("have no common ancestors and can not be compared."));
		}

		[Test]
		public void GivenNoDifferences_WhenBeingAsked_ThenReturnEmptyWithExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.Greater(answer.Explanation.Statements.Count, 0);

			Assert.IsTrue(text.Contains("No differences found according to existing information."));
		}

		[Test]
		public void GivenAllSetOnTheSameLevel_WhenBeingAsked_ThenReturnAllDifferences()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_JetFighter);

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var signs = ((ConceptsAnswer) answer).Result;
			Assert.AreEqual(2, signs.Count);
			Assert.IsTrue(signs.Contains(semanticNetwork.Sign_AreaType));
			Assert.IsTrue(signs.Contains(semanticNetwork.Sign_MotorType));

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}

		[Test]
		public void GivenAllSetOnTheDifferentLevels_WhenBeingAsked_ThenReturnAllDifferencesManyLevels()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			GetCommonQuestionTest.CreateCompareConceptsTest(semanticNetwork);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts["Concept 1"],
				semanticNetwork.Concepts["Concept 2"]);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.AreEqual(3, signs.Count);
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignDifferentValues));
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignFirstNotSet));
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignSecondNotSet));

			Assert.Greater(answer.Explanation.Statements.Count, 0);

			Assert.IsTrue(
				text.Contains("Result of ") &&
				text.Contains(" comparison:") &&
				text.Contains("sign value equal") &&
				text.Contains(", and second one equal to "));
		}

		[Test]
		public void GivenDifferentHierarchyLevels_WhenBeingAsked_ThenReturnAdditionalHierarchies()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			GetCommonQuestionTest.CreateCompareConceptsTest(semanticNetwork);

			const string sideBranchId = "SIDE_BRANCH";
			var sideBranch = sideBranchId.CreateConcept();
			semanticNetwork.Concepts.Add(sideBranch);
			semanticNetwork.DeclareThat(sideBranch).IsDescendantOf(semanticNetwork.Concepts["Parent 2"]);

			var render = TextRenders.PlainString;

			// act
			var answerFirst = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts["Concept 1"],
				semanticNetwork.Concepts[sideBranchId]);
			var textFirst = render.RenderText(answerFirst.Description, language).ToString();

			var answerSecond = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts[sideBranchId],
				semanticNetwork.Concepts["Concept 1"]);
			var textSecond = render.RenderText(answerSecond.Description, language).ToString();

			// assert
			Assert.IsFalse(answerFirst.IsEmpty);
			Assert.IsFalse(answerSecond.IsEmpty);

			Assert.Greater(answerFirst.Explanation.Statements.Count, 0);
			Assert.Greater(answerSecond.Explanation.Statements.Count, 0);

			Assert.IsTrue(textFirst.Contains("First is also:"));
			Assert.IsTrue(textSecond.Contains("Second is also:"));
		}
	}
}
