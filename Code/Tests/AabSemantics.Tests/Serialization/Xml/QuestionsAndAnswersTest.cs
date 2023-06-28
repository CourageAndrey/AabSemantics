using System.IO;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Tests.Serialization.Xml
{
	[TestFixture]
	public class QuestionsAndAnswersTest : Serialization.QuestionsAndAnswersTest
	{
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

			var restored = restoredXml.Save(ConceptIdResolver, StatementIdResolver);

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
			var xmlAnswer = AabSemantics.Serialization.Xml.Answer.Load(answer, Language);
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

			var restored = restoredXml.Save(ConceptIdResolver, StatementIdResolver);

			// assert
			foreach (var property in propertiesToCompare)
			{
				AssertEqual(property, answer, restored);
			}
		}
	}
}
