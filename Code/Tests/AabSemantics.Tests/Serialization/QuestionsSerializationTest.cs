using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Serialization;
using AabSemantics.Statements;
using AabSemantics.Questions;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Serialization
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

			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();
			_semanticNetwork = new SemanticNetwork(_language)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();
			IConcept a, d;
			_semanticNetwork.Concepts.Add(a = "a".CreateConceptByName());
			_semanticNetwork.Concepts.Add(d = "d".CreateConceptByName());
			_semanticNetwork.DeclareThat(a).IsAncestorOf(d);

			IConcept aa, dd;
			_semanticNetwork.Concepts.Add(aa = "aa".CreateConceptByName());
			_semanticNetwork.Concepts.Add(dd = "dd".CreateConceptByName());
			_semanticNetwork.DeclareThat(aa).IsAncestorOf(dd);

			_conceptIdResolver = new ConceptIdResolver(_semanticNetwork.Concepts.ToDictionary(
				concept => concept.ID,
				concept => concept));
			_statementIdResolver = new StatementIdResolver(_semanticNetwork);

			Repositories.RegisterCustomStatement(
				"type",
				new[] { "concept1", "concept2" },
				l => string.Empty,
				l => string.Empty,
				l => string.Empty);
		}

		[SetUp]
		public void SetUp()
		{
			Repositories.RegisterCustomStatement(
				"type",
				new[] { "concept1", "concept2" },
				l => string.Empty,
				l => string.Empty,
				l => string.Empty);
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
			var testStatement = _semanticNetwork.Statements.First();
			var testConcept1 = _semanticNetwork.Concepts["a"];
			var testConcept2 = _semanticNetwork.Concepts["d"];

			yield return new CheckStatementQuestion(testStatement, _semanticNetwork.Statements.Except(new[] { testStatement }));
			yield return new CustomStatementQuestion("type", new Dictionary<string, IConcept>
			{
				{ "concept1", testConcept1 },
				{ "concept2", testConcept2 },
			});
			yield return new EnumerateAncestorsQuestion(testConcept1);
			yield return new EnumerateDescendantsQuestion(testConcept1);
			yield return new IsQuestion(testConcept1, testConcept2);
			yield return new CustomStatementQuestion("type", new Dictionary<string, IConcept>{ { "concept1", testConcept1 }, { "concept2", testConcept2 } });
		}
	}
}
