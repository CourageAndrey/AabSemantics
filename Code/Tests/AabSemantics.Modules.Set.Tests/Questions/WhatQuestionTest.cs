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
	public class WhatQuestionTest
	{
		[Test]
		public void GivenWhatIs_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateSetTestData();

			var render = TextRenders.PlainString;

			// act
			var questionRegular = new WhatQuestion(semanticNetwork.Vehicle_Car);
			var answerRegular = questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = semanticNetwork.SemanticNetwork.Ask().WhatIs(semanticNetwork.Vehicle_Car);

			// assert
			Assert.AreEqual(render.RenderText(answerRegular.Description, language).ToString(), render.RenderText(answerBuilder.Description, language).ToString());
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}
	}
}
