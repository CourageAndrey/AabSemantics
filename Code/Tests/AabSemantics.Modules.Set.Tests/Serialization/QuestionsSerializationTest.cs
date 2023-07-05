using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Serialization;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Set.Tests.Serialization
{
	[TestFixture]
	public class QuestionsSerializationTest
	{
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;
		private static readonly ConceptIdResolver _conceptIdResolver;
		private static readonly StatementIdResolver _statementIdResolver;

		static QuestionsSerializationTest()
		{
			_language = Language.Default;

			_semanticNetwork = new SemanticNetwork(_language);
			_semanticNetwork.CreateSetTestData();

			_conceptIdResolver = new ConceptIdResolver(_semanticNetwork.Concepts.ToDictionary(
				concept => concept.ID,
				concept => concept));
			_statementIdResolver = new StatementIdResolver(_semanticNetwork);
		}

		[Test]
		[TestCaseSource(nameof(CreateQuestions))]
		public void GivenDifferentQuestions_WhenSerializeToJson_ThenSucceed(IQuestion question)
		{
			question.CheckJsonSerialization(_conceptIdResolver, _statementIdResolver);
		}

		[Test]
		[TestCaseSource(nameof(CreateQuestions))]
		public void GivenDifferentQuestions_WhenSerializeToXml_ThenSucceed(IQuestion question)
		{
			question.CheckXmlSerialization(_conceptIdResolver, _statementIdResolver);
		}

		public static IEnumerable<IQuestion> CreateQuestions()
		{
			var testConcept1 = _semanticNetwork.Concepts.First();
			var testConcept2 = _semanticNetwork.Concepts.Last();

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
