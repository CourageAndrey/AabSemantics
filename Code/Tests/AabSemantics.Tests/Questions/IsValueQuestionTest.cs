using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Test.Sample;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class IsValueQuestionTest
	{
		[Test]
		public void GivenNoAttribute_WhenBeingAsked_ThenReturnFalse()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var concept = ConceptCreationHelper.CreateConcept();
			semanticNetwork.Concepts.Add(concept);

			// act
			var answer = semanticNetwork.Ask().IfIsValue(concept);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsFalse(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenNoStatements_WhenBeingAsked_ThenReturnTrueWithoutExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var concept = ConceptCreationHelper.CreateConcept();
			concept.WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);

			// act
			var answer = semanticNetwork.Ask().IfIsValue(concept);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void GivenExistingStatements_WhenBeingAsked_ThenReturnTrueWithExplanation()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language);

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().IfIsValue(semanticNetwork.MotorType_Combusion);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(((BooleanAnswer) answer).Result);
			Assert.IsTrue(answer.Explanation.Statements.OfType<SignValueStatement>().Any());
		}
	}
}
