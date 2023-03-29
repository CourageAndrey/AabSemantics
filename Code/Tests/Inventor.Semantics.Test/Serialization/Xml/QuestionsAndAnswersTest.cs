using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Mathematics.Localization;
using Inventor.Semantics.Mathematics.Questions;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Modules.Classification.Questions;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Processes.Localization;
using Inventor.Semantics.Processes.Questions;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Questions;
using Inventor.Semantics.Test.Sample;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Test.Serialization.Xml
{
	[TestFixture]
	public class QuestionsAndAnswersTest
	{
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;
		private static readonly ConceptIdResolver _conceptIdResolver;

		static QuestionsAndAnswersTest()
		{
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
		}

		[Test]
		[TestCaseSource(nameof(createQuestions))]
		public void CheckQuestionsSerialization(IQuestion question)
		{
			// arrange
			var propertiesToCompare = question
				.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

			// act
			var xmlQuestion = Semantics.Serialization.Xml.Question.Load(question);
			var xml = xmlQuestion.SerializeToXmlElement().OuterXml;

			var serializer = xmlQuestion.GetType().AcquireXmlSerializer();
			Semantics.Serialization.Xml.Question restoredXml;
			using (var stringReader = new StringReader(xml))
			{
				using (var xmlStringReader = new XmlTextReader(stringReader))
				{
					restoredXml = (Semantics.Serialization.Xml.Question) serializer.Deserialize(xmlStringReader);
				}
			}

			var restored = restoredXml.Save(_conceptIdResolver);

			// assert
			Assert.AreSame(question.GetType(), restored.GetType());
			foreach (var property in propertiesToCompare)
			{
				AssertEqual(property, question, restored);
			}
		}

		private static void AssertEqual(PropertyInfo property, object left, object right)
		{
			object leftValue = property.GetValue(left);
			object rightValue = property.GetValue(right);

			if (property.PropertyType == typeof(bool) ||
				typeof(IStatement).IsAssignableFrom(property.PropertyType)) // because statements are loaded
#warning Looks like we need to resolve existing statements too.
			{
				Assert.AreEqual(leftValue, rightValue);
			}
			else if (typeof(IConcept).IsAssignableFrom(property.PropertyType)) // because concepts are resolved by ID
			{
				Assert.AreSame(leftValue, rightValue);
			}
			else if (typeof(IEnumerable<IStatement>).IsAssignableFrom(property.PropertyType))
			{
				var leftCollection = (IEnumerable<IStatement>) leftValue;
				var rightCollection = (IEnumerable<IStatement>) rightValue;
				Assert.IsTrue(leftCollection.SequenceEqual(rightCollection));
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private static IEnumerable<IQuestion> createQuestions()
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
	}
}
