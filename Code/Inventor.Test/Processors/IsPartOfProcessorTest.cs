using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Processors
{
	public class IsPartOfProcessorTest
	{
		[Test]
		public void ReturnFalseIfNotFound()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new IsPartOfQuestion(knowledgeBase.Part_Engine, knowledgeBase.Base_Vehicle);

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

			var question = new IsPartOfQuestion(knowledgeBase.Part_Engine, knowledgeBase.Vehicle_Car);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var statement = (HasPartStatement) answer.Explanation.Statements.Single();
			Assert.IsTrue(statement.Whole == knowledgeBase.Vehicle_Car && statement.Part == knowledgeBase.Part_Engine);
		}
	}
}
