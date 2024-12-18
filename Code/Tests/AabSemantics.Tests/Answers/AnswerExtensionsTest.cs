using System;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Text.Primitives;
using AabSemantics.Text.Renders;

namespace AabSemantics.Tests.Answers
{
	[TestFixture]
	public class AnswerExtensionsTest
	{
		[Test]
		public void GivenNoExplanation_WhenGetDescriptionWithExplanation_ThenReturnPlainDescription()
		{
			// arrange
			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());

			var render = new PlainStringTextRender();

			var text = new FormattedText(l => "Just text answer.");
			string initialText = render.Render(text, language).ToString();

			// act
			var answer = new BooleanAnswer(true, text, new Explanation(Array.Empty<IStatement>()));
			var explainedAnswer = answer.GetDescriptionWithExplanation();
			string explainedText = render.Render(explainedAnswer, language).ToString();

			// assert
			Assert.That(explainedText, Is.EqualTo(initialText));
		}

		[Test]
		public void GivenExplanationStatements_WhenGetDescriptionWithExplanation_ThenReturnExplainedDescription()
		{
			// arrange
			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());

			var render = new PlainStringTextRender();

			var text = new FormattedText(l => "Some answer, which required detailed explanation.");
			string initialText = render.Render(text, language).ToString();

			var concept1 = 1.CreateConceptByObject();
			var concept2 = 2.CreateConceptByObject();
			var concept3 = 3.CreateConceptByObject();
			var concept4 = 4.CreateConceptByObject();
			var statements = new IStatement[]
			{
				new IsStatement("12", concept1, concept2),
				new IsStatement("23", concept2, concept3),
				new IsStatement("34", concept3, concept4),
			};

			// act
			var answer = new BooleanAnswer(true, text, new Explanation(statements));
			var explainedAnswer = answer.GetDescriptionWithExplanation();
			string explainedText = render.Render(explainedAnswer, language).ToString();

			// assert
			Assert.That(explainedText.Contains(initialText), Is.True);
			Assert.That(explainedText.Contains(language.Questions.Answers.Explanation), Is.True);
			foreach (var statement in statements)
			{
				string statementText = render.Render(statement.DescribeTrue(), language).ToString();
				Assert.That(explainedText.Contains(statementText), Is.True);
			}
		}
	}
}
