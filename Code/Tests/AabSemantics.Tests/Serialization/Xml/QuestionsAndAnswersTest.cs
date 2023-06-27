using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;
using AabSemantics.Test.Sample;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Serialization.Xml
{
	[TestFixture]
	public class QuestionsAndAnswersTest
	{
		private static readonly ITextRepresenter _textRepresenter;
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;
		private static readonly ConceptIdResolver _conceptIdResolver;
		private static readonly StatementIdResolver _statementIdResolver;

		static QuestionsAndAnswersTest()
		{
			_textRepresenter = TextRepresenters.PlainString;

			_language = Language.Default;
			_language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			_language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			_language.Extensions.Add(LanguageSetModule.CreateDefault());
			_language.Extensions.Add(LanguageProcessesModule.CreateDefault());
			_language.Extensions.Add(LanguageMathematicsModule.CreateDefault());

			_semanticNetwork = new TestSemanticNetwork(_language).SemanticNetwork;

			_conceptIdResolver = new ConceptIdResolver(_semanticNetwork.Concepts.ToDictionary(
				concept => concept.ID,
				concept => concept));
			_statementIdResolver = new StatementIdResolver(_semanticNetwork);
		}

		[Test]
		[TestCaseSource(nameof(CreateQuestions))]
		public void GivenDifferentQuestions_WhenSerializeDeserialize_ThenSucceed(IQuestion question)
		{
			// arrange
			var propertiesToCompare = question
				.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

			// act
			var xmlQuestion = Question.Load(question);
			var xml = xmlQuestion.SerializeToXmlString();

			var serializer = xmlQuestion.GetType().AcquireXmlSerializer();
			Question restoredXml;
			using (var stringReader = new StringReader(xml))
			{
				using (var xmlStringReader = new XmlTextReader(stringReader))
				{
					restoredXml = (Question) serializer.Deserialize(xmlStringReader);
				}
			}

			var restored = restoredXml.Save(_conceptIdResolver, _statementIdResolver);

			// assert
			Assert.AreSame(question.GetType(), restored.GetType());
			foreach (var property in propertiesToCompare)
			{
				AssertEqual(property, question, restored);
			}
		}

		[Test]
		[TestCaseSource(nameof(CreateAnswers))]
		public void GivenDifferentAnswers_WhenSerializeDeserialize_ThenSucceed(IAnswer answer)
		{
			// arrange
			var propertiesToCompare = answer
				.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

			// act
			var xmlAnswer = AabSemantics.Serialization.Xml.Answer.Load(answer, _language);
			var xml = xmlAnswer.SerializeToXmlString();

			var serializer = xmlAnswer.GetType().AcquireXmlSerializer();
			AabSemantics.Serialization.Xml.Answer restoredXml;
			using (var stringReader = new StringReader(xml))
			{
				using (var xmlStringReader = new XmlTextReader(stringReader))
				{
					restoredXml = (AabSemantics.Serialization.Xml.Answer) serializer.Deserialize(xmlStringReader);
				}
			}

			var restored = restoredXml.Save(_conceptIdResolver, _statementIdResolver);

			// assert
			foreach (var property in propertiesToCompare)
			{
				AssertEqual(property, answer, restored);
			}
		}

		private static void AssertEqual(PropertyInfo property, object left, object right)
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
				var leftString = _textRepresenter.RepresentText(leftText, _language).ToString();
				var rightString = _textRepresenter.RepresentText(rightText, _language).ToString();
				Assert.AreEqual(leftString, rightString);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private static IEnumerable<IQuestion> CreateQuestions()
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

		private static IEnumerable<IAnswer> CreateAnswers()
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
