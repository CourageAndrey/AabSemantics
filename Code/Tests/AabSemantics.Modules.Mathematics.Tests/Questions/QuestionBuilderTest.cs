using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Mathematics.Tests.Questions
{
	[TestFixture]
	public class QuestionBuilderTest
	{
		[Test]
		public void GivenHowCompared_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			// act
			var questionRegular = new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number2);
			var answerRegular = (StatementAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (StatementAnswer) semanticNetwork.SemanticNetwork.Ask().HowCompared(semanticNetwork.Number0, semanticNetwork.Number2);

			// assert
			Assert.AreEqual(answerRegular.Result, answerBuilder.Result);
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}
	}
}
