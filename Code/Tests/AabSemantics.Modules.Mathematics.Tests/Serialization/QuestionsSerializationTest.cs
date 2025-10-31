using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Serialization;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Mathematics.Tests.Serialization
{
	[TestFixture]
	public class QuestionsSerializationTest
	{
		private static ILanguage _language;
		private static ISemanticNetwork _semanticNetwork;
		private static ConceptIdResolver _conceptIdResolver;
		private static StatementIdResolver _statementIdResolver;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			Initialize();
		}

		private static void Initialize()
		{
			if (_language != null) return;

			_language = Language.Default;

			_semanticNetwork = new SemanticNetwork(_language);
			_semanticNetwork.CreateMathematicsTestData();

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
			Initialize();

			var testConcept1 = _semanticNetwork.Concepts.First();
			var testConcept2 = _semanticNetwork.Concepts.Last();

			yield return new ComparisonQuestion(testConcept1, testConcept2);
		}
	}
}
