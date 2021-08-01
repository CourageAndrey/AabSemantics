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
	public class SignValueQuestionTest
	{
		[Test]
		public void ReturnUnknownIfNoInformation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);
			var question = new SignValueQuestion(semanticNetwork.Base_Vehicle, semanticNetwork.Sign_AreaType);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void ReturnDirectSignValue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);
			var question = new SignValueQuestion(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_AreaType);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreSame(typeof(SignValueStatement), answer.Explanation.Statements.Single().GetType());
		}

		[Test]
		public void ReturnInheritedSignValue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var truck = new Concept();
			semanticNetwork.SemanticNetwork.Concepts.Add(truck);

			var classification = new IsStatement(semanticNetwork.Vehicle_Car, truck);
			semanticNetwork.SemanticNetwork.Statements.Add(classification);

			var question = new SignValueQuestion(truck, semanticNetwork.Sign_AreaType);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Ground, ((ConceptAnswer) answer).Result);
			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
			Assert.AreEqual(1, answer.Explanation.Statements.OfType<SignValueStatement>().Count());
		}

		[Test]
		public void ReturnOverridenSignValue()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var flyingCar = new Concept();
			semanticNetwork.SemanticNetwork.Concepts.Add(flyingCar);

			var classification = new IsStatement(semanticNetwork.Vehicle_Car, flyingCar);
			semanticNetwork.SemanticNetwork.Statements.Add(classification);

			var newValue = new SignValueStatement(flyingCar, semanticNetwork.Sign_AreaType, semanticNetwork.AreaType_Air);
			semanticNetwork.SemanticNetwork.Statements.Add(newValue);

			var question = new SignValueQuestion(flyingCar, semanticNetwork.Sign_AreaType);

			// act
			var answer = question.Ask(semanticNetwork.SemanticNetwork.Context);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Air, ((ConceptAnswer) answer).Result);
			Assert.AreSame(newValue, answer.Explanation.Statements.Single());
		}
	}
}
