using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class GetDifferencesQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConcept();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new GetDifferencesQuestion(concept, null));
		}

		[Test]
		public void GivenSameArguments_WhenCreateQuestion_ThenFail()
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

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.AreaType_Air, semanticNetwork.MotorType_Jet);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenNoDifferences_WhenBeingAsked_ThenReturnEmptyWithExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsTheDifference(semanticNetwork.Vehicle_Car, semanticNetwork.Vehicle_Motorcycle);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.Greater(answer.Explanation.Statements.Count, 0);
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

			// act
			var answer = semanticNetwork.Ask().WhatIsTheDifference(
				semanticNetwork.Concepts.First(c => c.HasAttribute<IsProcessAttribute>()),
				semanticNetwork.Concepts.Last(c => c.HasAttribute<IsProcessAttribute>()));

			// assert
			Assert.IsFalse(answer.IsEmpty);

			var signs = (((ConceptsAnswer) answer).Result).Select(c => c.Name.GetValue(language)).ToList();
			Assert.AreEqual(3, signs.Count);
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignDifferentValues));
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignFirstNotSet));
			Assert.IsTrue(signs.Contains(GetCommonQuestionTest.SignSecondNotSet));

			Assert.Greater(answer.Explanation.Statements.Count, 0);
		}
	}
}
