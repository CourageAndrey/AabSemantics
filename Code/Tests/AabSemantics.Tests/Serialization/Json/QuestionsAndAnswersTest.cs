using System.IO;
using System.Reflection;
using System.Text;

using NUnit.Framework;

using AabSemantics.Serialization.Json;

namespace AabSemantics.Tests.Serialization.Json
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
			var jsonQuestion = Question.Load(question);
			var json = jsonQuestion.SerializeToJsonString();

			var serializer = jsonQuestion.GetType().AcquireJsonSerializer();
			Question restoredJson;
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				restoredJson = (Question) serializer.ReadObject(stream);
			}

			var restored = restoredJson.Save(ConceptIdResolver, StatementIdResolver);

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
			var jsonAnswer = Answer.Load(answer, Language);
			var json = jsonAnswer.SerializeToJsonString();

			var serializer = jsonAnswer.GetType().AcquireJsonSerializer();
			Answer restoredJson;
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				restoredJson = (Answer) serializer.ReadObject(stream);
			}

			var restored = restoredJson.Save(ConceptIdResolver, StatementIdResolver);

			// assert
			foreach (var property in propertiesToCompare)
			{
				AssertEqual(property, answer, restored);
			}
		}
	}
}
