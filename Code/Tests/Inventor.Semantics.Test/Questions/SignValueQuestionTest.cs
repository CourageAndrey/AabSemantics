using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Modules.Set.Questions;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Questions
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

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Base_Vehicle, semanticNetwork.Sign_AreaType);

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

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(semanticNetwork.Vehicle_Car, semanticNetwork.Sign_AreaType);

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

			var truck = ConceptCreationHelper.CreateConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(truck);

			var classification = semanticNetwork.SemanticNetwork.DeclareThat(truck).IsDescendantOf(semanticNetwork.Vehicle_Car);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(truck, semanticNetwork.Sign_AreaType);

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

			var flyingCar = ConceptCreationHelper.CreateConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(flyingCar);

			semanticNetwork.SemanticNetwork.DeclareThat(flyingCar).IsDescendantOf(semanticNetwork.Vehicle_Car);

			var newValue = semanticNetwork.SemanticNetwork.DeclareThat(semanticNetwork.AreaType_Air).IsSignValue(flyingCar, semanticNetwork.Sign_AreaType);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().WhatIsSignValue(flyingCar, semanticNetwork.Sign_AreaType);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.AreSame(semanticNetwork.AreaType_Air, ((ConceptAnswer) answer).Result);
			Assert.AreSame(newValue, answer.Explanation.Statements.Single());
		}
	}
}
