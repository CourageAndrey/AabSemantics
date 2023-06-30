using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;

namespace AabSemantics.Modules.Processes.Tests.Questions
{
	[TestFixture]
	public class SimpleAnswersTest
	{
		private static readonly ITextRender _textRender;
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;

		static SimpleAnswersTest()
		{
			_textRender = TextRenders.PlainString;

			_language = Language.Default;
			_language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			_language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			_language.Extensions.Add(LanguageProcessesModule.CreateDefault());

			_semanticNetwork = new SemanticNetwork(_language);
		}

		[Test]
		[TestCaseSource(nameof(CreateEmptyQuestions))]
		public void GivenQuestionsForEmpty_WhenBeingAsked_ThenAnswerCorrespondingly(IQuestion question)
		{
			// act
			var answer = question.Ask(_semanticNetwork.Context);
			var description = _textRender.RenderText(answer.Description, _language).ToString();

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.IsTrue(description.Contains(_language.Questions.Answers.Unknown));
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		private static IEnumerable<object[]> CreateEmptyQuestions()
		{
			var concept1 = 1.CreateConcept();
			var concept2 = 2.CreateConcept();

			yield return new object[] { new ProcessesQuestion(concept1, concept2) };
		}
	}
}
