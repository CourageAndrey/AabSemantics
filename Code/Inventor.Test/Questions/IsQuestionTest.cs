using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class IsQuestionTest
	{
		[Test]
		public void AnswerNotFound()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(semanticNetwork.Sign_AreaType, semanticNetwork.Base_Vehicle);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void FindCertainStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(semanticNetwork.Vehicle_Airbus, semanticNetwork.Base_Vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			var classification = (IsStatement) answer.Explanation.Statements.Single();
			Assert.AreSame(semanticNetwork.Base_Vehicle, classification.Ancestor);
			Assert.AreSame(semanticNetwork.Vehicle_Airbus, classification.Descendant);
		}

		[Test]
		public void FindWithTransition()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			var hugeAirbus = TestHelper.CreateConcept();
			semanticNetwork.SemanticNetwork.Concepts.Add(hugeAirbus);

			var classification = semanticNetwork.SemanticNetwork.DeclareThat(hugeAirbus).IsDescendantOf(semanticNetwork.Vehicle_Airbus);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIs(hugeAirbus, semanticNetwork.Base_Vehicle);

			//assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);

			Assert.AreEqual(2, answer.Explanation.Statements.Count);
			Assert.AreEqual(2, answer.Explanation.Statements.OfType<IsStatement>().Count());
			Assert.IsTrue(answer.Explanation.Statements.Contains(classification));
		}
	}
}
