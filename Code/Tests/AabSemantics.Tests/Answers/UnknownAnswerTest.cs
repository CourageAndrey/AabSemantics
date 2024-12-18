using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;

namespace AabSemantics.Tests.Answers
{
	[TestFixture]
	public class UnknownAnswerTest
	{
		[Test]
		public void GivenEmptyAnswer_WhenCheckIt_ThenItIsEmpty()
		{
			// arrange
			var language = Language.Default;
			var render = TextRenders.PlainString;

			// act
			var emptyAnswer = Answer.CreateUnknown();
			string representation = render.Render(emptyAnswer.Description, language).ToString();

			// assert
			Assert.That(emptyAnswer.IsEmpty, Is.True);
			Assert.That(representation.Contains(language.Questions.Answers.Unknown), Is.True);
			Assert.That(emptyAnswer.Explanation.Statements.Count, Is.EqualTo(0));
		}
	}
}
