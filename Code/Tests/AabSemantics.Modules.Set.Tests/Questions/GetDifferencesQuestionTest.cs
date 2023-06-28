using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Questions;
using AabSemantics.Test.Sample;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class GetDifferencesQuestionTest
	{
		[Test]
		public void GivenConceptsCanNotBeCompared_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

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
			var semanticNetwork = new TestSemanticNetwork(language);

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
			var semanticNetwork = new TestSemanticNetwork(language);

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
