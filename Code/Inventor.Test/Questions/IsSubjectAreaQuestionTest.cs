using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class IsSubjectAreaQuestionTest
	{
		[Test]
		public void ReturnFalseIfNotFound()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new IsSubjectAreaQuestion(knowledgeBase.SubjectArea_Numbers, knowledgeBase.Base_Vehicle);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnTrueIfFound()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new IsSubjectAreaQuestion(knowledgeBase.Base_Vehicle, knowledgeBase.SubjectArea_Transport);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (GroupStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Concept == knowledgeBase.Base_Vehicle && statement.Area == knowledgeBase.SubjectArea_Transport);
		}
	}
}
