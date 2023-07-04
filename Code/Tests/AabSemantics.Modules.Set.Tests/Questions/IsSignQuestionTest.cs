using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class IsSignQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// act && assert
			Assert.Throws<ArgumentNullException>(() => new IsSignQuestion(null));
		}

		[Test]
		public void GivenIfIsSign_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			// act
			var questionRegular = new IsSignQuestion(semanticNetwork.Sign_AreaType);
			var answerRegular = (BooleanAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (BooleanAnswer) semanticNetwork.SemanticNetwork.Ask().IfIsSign(semanticNetwork.Sign_AreaType);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoAttribute_WhenBeingAsked_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData().SemanticNetwork;

			var concept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(concept);

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.Ask().IfIsSign(concept);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);

			Assert.IsTrue(text.Contains(" is not sign."));
		}

		[Test]
		public void GivenNoStatements_WhenBeingAsked_ThenReturnTrueWithoutExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData().SemanticNetwork;

			var concept = ConceptCreationHelper.CreateConcept();
			concept.WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(concept);

			// act
			var answer = semanticNetwork.Ask().IfIsSign(concept);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenExistingStatements_WhenBeingAsked_ThenReturnTrueWithExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsSign(semanticNetwork.Sign_AreaType);
			var text = render.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.IsTrue(answer.Explanation.Statements.OfType<HasSignStatement>().Any());

			Assert.IsTrue(text.Contains(" is sign."));
		}
	}
}
