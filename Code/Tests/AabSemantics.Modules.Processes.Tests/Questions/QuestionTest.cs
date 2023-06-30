using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Processes.Questions;

namespace AabSemantics.Modules.Processes.Tests.Questions
{
	[TestFixture]
	public class QuestionTest
	{
		[Test]
		[TestCaseSource(nameof(CreateQuestionsArgumentNullException))]
		public void GivenNullArguments_WhenCreateQuestions_ThenFail(Func<IQuestion> constructor)
		{
			Assert.Throws<ArgumentNullException>(() => constructor());
		}

		private static IEnumerable<object[]> CreateQuestionsArgumentNullException()
		{
			IConcept concept = "test".CreateConcept();

			yield return new object[] { new Func<IQuestion>(() => new ProcessesQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new ProcessesQuestion(concept, null)) };
		}
	}
}
