using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Tests;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Tests;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Tests;
using AabSemantics.Serialization;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Serialization
{
	public abstract class QuestionsAndAnswersTest
	{
		private static readonly ITextRender _textRender;
		private static readonly ISemanticNetwork _semanticNetwork;
		protected static readonly ILanguage Language;
		protected static readonly ConceptIdResolver ConceptIdResolver;
		protected static readonly StatementIdResolver StatementIdResolver;

		static QuestionsAndAnswersTest()
		{
			_textRender = TextRenders.PlainString;

			Language = AabSemantics.Localization.Language.Default;
			Language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			Language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			Language.Extensions.Add(LanguageSetModule.CreateDefault());
			Language.Extensions.Add(LanguageProcessesModule.CreateDefault());
			Language.Extensions.Add(LanguageMathematicsModule.CreateDefault());

			_semanticNetwork = new SemanticNetwork(Language);
			_semanticNetwork.CreateSetTestData();
			_semanticNetwork.CreateMathematicsTestData();
			_semanticNetwork.CreateProcessesTestData();

			ConceptIdResolver = new ConceptIdResolver(_semanticNetwork.Concepts.ToDictionary(
				concept => concept.ID,
				concept => concept));
			StatementIdResolver = new StatementIdResolver(_semanticNetwork);
		}

		protected static void AssertEqual(PropertyInfo property, object left, object right)
		{
			object leftValue = property.GetValue(left);
			object rightValue = property.GetValue(right);
			Type propertyType = property.PropertyType;

			if (typeof(IExplanation).IsAssignableFrom(propertyType))
			{
				leftValue = ((IExplanation) leftValue).Statements;
				rightValue = ((IExplanation) rightValue).Statements;
				propertyType = typeof(ICollection<IStatement>);
			}

			if (propertyType == typeof(bool))
			{
				Assert.AreEqual(leftValue, rightValue);
			}
			else if (typeof(IConcept).IsAssignableFrom(propertyType) ||
					typeof(IStatement).IsAssignableFrom(propertyType))
			{
				Assert.AreSame(leftValue, rightValue);
			}
			else if (typeof(IEnumerable<IConcept>).IsAssignableFrom(propertyType))
			{
				var leftCollection = (IEnumerable<IConcept>) leftValue;
				var rightCollection = (IEnumerable<IConcept>) rightValue;
				Assert.IsTrue(leftCollection.SequenceEqual(rightCollection));
			}
			else if (typeof(IEnumerable<IStatement>).IsAssignableFrom(propertyType))
			{
				var leftCollection = (IEnumerable<IStatement>) leftValue;
				var rightCollection = (IEnumerable<IStatement>) rightValue;
				Assert.IsTrue(leftCollection.SequenceEqual(rightCollection));
			}
			else if (typeof(IText).IsAssignableFrom(propertyType))
			{
				var leftText = (IText) leftValue;
				var rightText = (IText) rightValue;
				var leftString = _textRender.RenderText(leftText, Language).ToString();
				var rightString = _textRender.RenderText(rightText, Language).ToString();
				Assert.AreEqual(leftString, rightString);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		protected static IEnumerable<IQuestion> CreateQuestions()
		{
			var testStatement = _semanticNetwork.Statements.First();
			var testConcept1 = _semanticNetwork.Concepts.First();
			var testConcept2 = _semanticNetwork.Concepts.Last();

			yield return new CheckStatementQuestion(testStatement, _semanticNetwork.Statements.Except(new[] { testStatement }));
			yield return new EnumerateAncestorsQuestion(testConcept1);
			yield return new EnumerateDescendantsQuestion(testConcept1);
			yield return new IsQuestion(testConcept1, testConcept2);
			yield return new ComparisonQuestion(testConcept1, testConcept2);
			yield return new ProcessesQuestion(testConcept1, testConcept2);
			yield return new DescribeSubjectAreaQuestion(testConcept1);
			yield return new EnumerateContainersQuestion(testConcept1);
			yield return new EnumeratePartsQuestion(testConcept1);
			yield return new EnumerateSignsQuestion(testConcept1, true);
			yield return new FindSubjectAreaQuestion(testConcept1);
			yield return new GetCommonQuestion(testConcept1, testConcept2);
			yield return new GetDifferencesQuestion(testConcept1, testConcept2);
			yield return new HasSignQuestion(testConcept1, testConcept2, true);
			yield return new HasSignsQuestion(testConcept1, true);
			yield return new IsPartOfQuestion(testConcept1, testConcept2);
			yield return new IsSignQuestion(testConcept1);
			yield return new IsSubjectAreaQuestion(testConcept1, testConcept2);
			yield return new IsValueQuestion(testConcept1);
			yield return new SignValueQuestion(testConcept1, testConcept2);
			yield return new WhatQuestion(testConcept1);
		}

		protected static IEnumerable<IAnswer> CreateAnswers()
		{
			var text = new FormattedText(
				language => "_#A#_",
				new Dictionary<string, IKnowledge>{ { "A", _semanticNetwork.Concepts.First() } });
			var explanation = new Explanation(_semanticNetwork.Statements);

			yield return new AabSemantics.Answers.Answer(text, explanation, true);
			yield return new BooleanAnswer(true, text, explanation);
			yield return new ConceptAnswer(_semanticNetwork.Concepts.First(), text, explanation);
			yield return new ConceptsAnswer(_semanticNetwork.Concepts, text, explanation);
			yield return new StatementAnswer(_semanticNetwork.Statements.First(), text, explanation);
			yield return new StatementsAnswer(_semanticNetwork.Statements, text, explanation);
		}
	}
}
