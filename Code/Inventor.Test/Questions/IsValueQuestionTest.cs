using System.Linq;

using NUnit.Framework;

using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class IsValueQuestionTest
	{
		[Test]
		public void ReturnFalseIfNoAttribute()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language).KnowledgeBase;

			var concept = new Concept();
			knowledgeBase.Concepts.Add(concept);

			var question = new IsValueQuestion(concept);

			// act
			var answer = question.Ask(knowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnTrueWithoutExplanationIfNoRelationships()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language).KnowledgeBase;

			var concept = new Concept();
			concept.Attributes.Add(IsValueAttribute.Value);
			knowledgeBase.Concepts.Add(concept);

			var question = new IsValueQuestion(concept);

			// act
			var answer = question.Ask(knowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnTrueWithtExplanation()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var question = new IsValueQuestion(knowledgeBase.MotorType_Combusion);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.IsTrue(answer.Explanation.Statements.OfType<SignValueStatement>().Any());
		}
	}
}
