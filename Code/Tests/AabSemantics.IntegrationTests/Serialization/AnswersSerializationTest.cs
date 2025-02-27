using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;
using AabSemantics.Serialization.Xml;
using AabSemantics.TestCore;
using AabSemantics.Text.Primitives;

namespace AabSemantics.IntegrationTests.Serialization
{
	[TestFixture]
	public class AnswersSerializationTest
	{
		private static readonly ILanguage _language;
		private static readonly ISemanticNetwork _semanticNetwork;
		private static readonly ConceptIdResolver _conceptIdResolver;
		private static readonly StatementIdResolver _statementIdResolver;

		static AnswersSerializationTest()
		{
			_language = AabSemantics.Localization.Language.Default;

			_semanticNetwork = new SemanticNetwork(_language);
			_semanticNetwork.CreateCombinedTestData();

			_conceptIdResolver = new ConceptIdResolver(_semanticNetwork.Concepts.ToDictionary(
				concept => concept.ID,
				concept => concept));
			_statementIdResolver = new StatementIdResolver(_semanticNetwork);
		}

		[Test]
		[TestCaseSource(nameof(CreateAnswers))]
		public void GivenDifferentAnswers_WhenSerializeToJson_ThenSucceed(IAnswer answer)
		{
			answer.CheckSerialization(
				item => AabSemantics.Serialization.Json.Answer.Load(answer, _language),
				snapshot => snapshot.SerializeToJsonString(),
				(serialized, snapshotType) =>
				{
					var serializer = snapshotType.AcquireJsonSerializer();
					using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialized)))
					{
						return (AabSemantics.Serialization.Json.Answer) serializer.ReadObject(stream);
					}
				},
				snapshot => snapshot.Save(_conceptIdResolver, _statementIdResolver));
		}

		[Test]
		[TestCaseSource(nameof(CreateAnswers))]
		public void GivenDifferentAnswers_WhenSerializeToXml_ThenSucceed(IAnswer answer)
		{
			answer.CheckSerialization(
				item => AabSemantics.Serialization.Xml.Answer.Load(answer, _language),
				snapshot => snapshot.SerializeToXmlString(),
				(serialized, snapshotType) =>
				{
					var serializer = snapshotType.AcquireXmlSerializer();
					using (var stringReader = new StringReader(serialized))
					{
						using (var xmlStringReader = new XmlTextReader(stringReader))
						{
							return (AabSemantics.Serialization.Xml.Answer) serializer.Deserialize(xmlStringReader);
						}
					}
				},
				snapshot => snapshot.Save(_conceptIdResolver, _statementIdResolver));
		}

		public static IEnumerable<IAnswer> CreateAnswers()
		{
			var text = new FormattedText(
				language => "_#A#_",
				new Dictionary<string, IKnowledge>{ { "A", _semanticNetwork.Concepts.First() } });
			var explanation = new Explanation(_semanticNetwork.Statements);

			yield return new Answers.Answer(text, explanation, true);
			yield return new BooleanAnswer(true, text, explanation);
			yield return new ConceptAnswer(_semanticNetwork.Concepts.First(), text, explanation);
			yield return new ConceptsAnswer(_semanticNetwork.Concepts, text, explanation);
			yield return new StatementAnswer(_semanticNetwork.Statements.First(), text, explanation);
			yield return new StatementsAnswer(_semanticNetwork.Statements, text, explanation);
		}
	}
}
