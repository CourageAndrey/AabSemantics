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
	[TestFixture]
	public class SignValueProcessorTest
	{
		[Test]
		public void ReturnUnknownIfNoInformation()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);
			var question = new SignValueQuestion(knowledgeBase.Base_Vehicle, knowledgeBase.Sign_AreaType);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnDirectSignValue()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);
			var question = new SignValueQuestion(knowledgeBase.Vehicle_Car, knowledgeBase.Sign_AreaType);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(knowledgeBase.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreSame(typeof(SignValueStatement), answer.Explanation.Statements.Single().GetType());
		}

		[Test]
		public void ReturnInheritedSignValue()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var truck = new Concept();
			knowledgeBase.KnowledgeBase.Concepts.Add(truck);

			var classification = new IsStatement(knowledgeBase.Vehicle_Car, truck);
			knowledgeBase.KnowledgeBase.Statements.Add(classification);

			var question = new SignValueQuestion(truck, knowledgeBase.Sign_AreaType);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(knowledgeBase.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
			Assert.AreEqual(1, answer.Explanation.Statements.OfType<SignValueStatement>().Count());
		}

		[Test]
		public void ReturnOverridenSignValue()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var flyingCar = new Concept();
			knowledgeBase.KnowledgeBase.Concepts.Add(flyingCar);

			var classification = new IsStatement(knowledgeBase.Vehicle_Car, flyingCar);
			knowledgeBase.KnowledgeBase.Statements.Add(classification);

			var newValue = new SignValueStatement(flyingCar, knowledgeBase.Sign_AreaType, knowledgeBase.AreaType_Air);
			knowledgeBase.KnowledgeBase.Statements.Add(newValue);

			var question = new SignValueQuestion(flyingCar, knowledgeBase.Sign_AreaType);

			// act
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(knowledgeBase.AreaType_Air, ((ConceptAnswer) answer).Result);
			Assert.AreSame(newValue, answer.Explanation.Statements.Single());
		}
	}
}
