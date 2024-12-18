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
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(concept, null));
		}

		[Test]
		public void GivenSameArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

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
			Assert.That(questionRegular.Concept1, Is.SameAs(semanticNetwork.Vehicle_Car));
			Assert.That(questionRegular.Concept2, Is.SameAs(semanticNetwork.Vehicle_Airbus));
			Assert.That(answerRegular.Result.SequenceEqual(answerBuilder.Result), Is.True);
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
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
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));

			Assert.That(text.Contains("have no common ancestors and can not be compared."), Is.True);
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
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(text.Contains("No differences found according to existing information."), Is.True);
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
			Assert.That(answer.IsEmpty, Is.False);

			var signs = ((ConceptsAnswer) answer).Result;
			Assert.That(signs.Count, Is.EqualTo(2));
			Assert.That(signs.Contains(semanticNetwork.Sign_AreaType), Is.True);
			Assert.That(signs.Contains(semanticNetwork.Sign_MotorType), Is.True);

			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));
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
			Assert.That(answer.IsEmpty, Is.False);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.That(signs.Count, Is.EqualTo(3));
			Assert.That(signs.Contains(GetCommonQuestionTest.SignDifferentValues), Is.True);
			Assert.That(signs.Contains(GetCommonQuestionTest.SignFirstNotSet), Is.True);
			Assert.That(signs.Contains(GetCommonQuestionTest.SignSecondNotSet), Is.True);

			Assert.That(answer.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(
				text.Contains("Result of ") &&
				text.Contains(" comparison:") &&
				text.Contains("sign value equal") &&
				text.Contains(", and second one equal to "), Is.True);
		}

		[Test]
		public void GivenDifferentHierarchyLevels_WhenBeingAsked_ThenReturnAdditionalHierarchies()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			GetCommonQuestionTest.CreateCompareConceptsTest(semanticNetwork);

			const string sideBranchId = "SIDE_BRANCH";
			var sideBranch = sideBranchId.CreateConceptByName();
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
			Assert.That(answerFirst.IsEmpty, Is.False);
			Assert.That(answerSecond.IsEmpty, Is.False);

			Assert.That(answerFirst.Explanation.Statements.Count, Is.GreaterThan(0));
			Assert.That(answerSecond.Explanation.Statements.Count, Is.GreaterThan(0));

			Assert.That(textFirst.Contains("First is also:"), Is.True);
			Assert.That(textSecond.Contains("Second is also:"), Is.True);
		}
	}
}
