using System;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Text.Primitives;
using AabSemantics.Text.Representers;

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
			language.Extensions.Add(LanguageSetModule.CreateDefault());

			var representer = new PlainStringTextRepresenter();

			var text = new FormattedText(l => "Just text answer.");
			string initialText = representer.Represent(text, language).ToString();

			// act
			var answer = new BooleanAnswer(true, text, new Explanation(Array.Empty<IStatement>()));
			var explainedAnswer = answer.GetDescriptionWithExplanation();
			string explainedText = representer.Represent(explainedAnswer, language).ToString();

			// assert
			Assert.AreEqual(initialText, explainedText);
		}

		[Test]
		public void GivenExplanationStatements_WhenGetDescriptionWithExplanation_ThenReturnExplainedDescription()
		{
			// arrange
			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			language.Extensions.Add(LanguageSetModule.CreateDefault());

			var representer = new PlainStringTextRepresenter();

			var text = new FormattedText(l => "Some answer, which required detailed explanation.");
			string initialText = representer.Represent(text, language).ToString();

			var concept = new Concept("CONCEPT", new LocalizedStringConstant(l => "CoNcEpT"));
			var statements = new IStatement[]
			{
				new HasPartStatement("HasPartStatement", concept, concept),
				new IsStatement("IsStatement", concept, concept),
				new GroupStatement("GroupStatement", concept, concept),
			};

			// act
			var answer = new BooleanAnswer(true, text, new Explanation(statements));
			var explainedAnswer = answer.GetDescriptionWithExplanation();
			string explainedText = representer.Represent(explainedAnswer, language).ToString();

			// assert
			Assert.IsTrue(explainedText.Contains(initialText));
			Assert.IsTrue(explainedText.Contains(language.Questions.Answers.Explanation));
			foreach (var statement in statements)
			{
				string statementText = representer.Represent(statement.DescribeTrue(), language).ToString();
				Assert.IsTrue(explainedText.Contains(statementText));
			}
		}
	}
}
