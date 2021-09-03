using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Test.Sample;

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
		public void ReturnTrueWithoutExplanationIfNoRelationships()
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
		public void ReturnTrueWithtExplanation()
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
