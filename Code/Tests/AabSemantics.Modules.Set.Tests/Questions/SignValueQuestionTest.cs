using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class SignValueQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new SignValueQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new SignValueQuestion(concept, null));
		}
		
		[Test]
		public void GivenWhatIsSignValue_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);
			var answerRegular = (ConceptAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (ConceptAnswer) semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_MotorType);

			// assert
			Assert.AreSame(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Base_Vehicle, semanticNetwork.Sign_AreaType);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenDirectSignValue_WhenBeingAsked_ThenReturnIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_AreaType);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreSame(typeof(SignValueStatement), answer.Explanation.Statements.Single().GetType());
		}

		[Test]
		public void GivenInheritedSignValue_WhenBeingAsked_ThenReturnIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var truck = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(truck);

			var classification = semanticNetwork.SemanticNetwork.DeclareThat(truck).IsDescendantOf(semanticNetwork.Vehicle_Car);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(truck, semanticNetwork.Sign_AreaType);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
			Assert.AreEqual(1, answer.Explanation.Statements.OfType<SignValueStatement>().Count());
		}

		[Test]
		public void GivenOverridenValue_WhenBeingAsked_ThenReturnIt()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var flyingCar = ConceptCreationHelper.CreateEmptyConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(flyingCar);

			semanticNetwork.SemanticNetwork.DeclareThat(flyingCar).IsDescendantOf(semanticNetwork.Vehicle_Car);

			var newValue = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.AreaType_Air).IsSignValue(flyingCar, semanticNetwork.Sign_AreaType);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(flyingCar, semanticNetwork.Sign_AreaType);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Air, ((ConceptAnswer) answer).Result);
			Assert.AreSame(newValue, answer.Explanation.Statements.Single());

			Assert.IsTrue(text.Contains(" concept has "));
		}
	}
}
