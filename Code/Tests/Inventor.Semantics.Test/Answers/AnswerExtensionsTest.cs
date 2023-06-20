using System;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Text.Primitives;
using Inventor.Semantics.Text.Representers;

namespace Inventor.Semantics.Test.Answers
{
	[TestFixture]
	public class AnswerExtensionsTest
	{
		[Test]
		public void GivenNoExplanationWhenGetDescriptionWithExplanationThenReturnPlainDescription()
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
		public void GivenExplanationStatementsWhenGetDescriptionWithExplanationThenReturnExplainedDescription()
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
