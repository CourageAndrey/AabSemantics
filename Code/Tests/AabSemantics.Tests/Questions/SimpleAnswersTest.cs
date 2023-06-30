using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class SimpleAnswersTest
	{
		private static readonly ITextRender _textRender;
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;
		private static readonly IConcept _concept1, _concept2, _concept3;

		static SimpleAnswersTest()
		{
			_textRender = TextRenders.PlainString;

			_language = Language.Default;
			_language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			_language.Extensions.Add(LanguageClassificationModule.CreateDefault());

			_semanticNetwork = new SemanticNetwork(_language);

			_semanticNetwork.Concepts.Add(_concept1 = 1.CreateConcept());
			_semanticNetwork.Concepts.Add(_concept2 = 2.CreateConcept());
			_semanticNetwork.Concepts.Add(_concept3 = 3.CreateConcept());

			_semanticNetwork.DeclareThat(_concept1).IsAncestorOf(_concept2);
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

		[Test]
		[TestCaseSource(nameof(CreateTrueQuestions))]
		public void GivenQuestionsForTrue_WhenBeingAsked_ThenAnswerCorrespondingly(IQuestion question, string expectedAnswerToken)
		{
			// act
			var answer = (BooleanAnswer) question.Ask(_semanticNetwork.Context);
			var description = _textRender.RenderText(answer.Description, _language).ToString();

			// assert
			Assert.IsTrue(answer.Result);
			Assert.IsTrue(description.Contains(expectedAnswerToken));
		}

		[Test]
		[TestCaseSource(nameof(CreateFalseQuestions))]
		public void GivenQuestionsForFalse_WhenBeingAsked_ThenAnswerCorrespondingly(IQuestion question, string expectedAnswerToken)
		{
			// act
			var answer = (BooleanAnswer) question.Ask(_semanticNetwork.Context);
			var description = _textRender.RenderText(answer.Description, _language).ToString();

			// assert
			Assert.IsFalse(answer.Result);
			Assert.IsTrue(description.Contains(expectedAnswerToken));
		}

		private static IEnumerable<object[]> CreateEmptyQuestions()
		{
			var concept1 = 1.CreateConcept();

			yield return new object[] { new EnumerateAncestorsQuestion(concept1) };
			yield return new object[] { new EnumerateDescendantsQuestion(concept1) };
		}

		private static IEnumerable<object[]> CreateTrueQuestions()
		{
			var statement = _semanticNetwork.Statements.First();

			yield return new object[] { new CheckStatementQuestion(statement), _textRender.RenderText(statement.DescribeTrue(), _language).ToString() };
			yield return new object[] { new IsQuestion(_concept2, _concept1), "\" is \"" };
		}

		private static IEnumerable<object[]> CreateFalseQuestions()
		{
			var statement = new IsStatement(null, _concept3, _concept2);

			yield return new object[] { new CheckStatementQuestion(statement), _textRender.RenderText(statement.DescribeFalse(), _language).ToString() };
			yield return new object[] { new IsQuestion(_concept1, _concept2), "\" is not \"" };
		}
	}
}
