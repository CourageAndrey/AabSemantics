using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class QuestionTest
	{
		[Test]
		public void ExplainPreconditions()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = TestHelper.CreateConcept();
			concept1.WithAttribute(IsValueAttribute.Value);
			var concept2 = TestHelper.CreateConcept();
			concept2.WithAttribute(IsValueAttribute.Value);
			var concept3 = TestHelper.CreateConcept();
			concept3.WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.Concepts.Add(concept3);

			var initialComparison = semanticNetwork.DeclareThat(concept1).IsEqualTo(concept2);

			var preconditionComparison = new ComparisonStatement(null, concept2, concept3, ComparisonSigns.IsEqualTo);
			// ... and do not add it to semantic network

			// act
			var answerWithoutPreconditions = semanticNetwork.Ask().HowCompared(concept1, concept3);
			var answerWithPreconditions = semanticNetwork.Supposing(new IStatement[] { preconditionComparison }).Ask().HowCompared(concept1, concept3);

			// assert
			Assert.IsTrue(answerWithoutPreconditions.IsEmpty);
			Assert.AreEqual(0, answerWithoutPreconditions.Explanation.Statements.Count);

			Assert.IsFalse(answerWithPreconditions.IsEmpty);
			var resultComparison = (ComparisonStatement) ((StatementAnswer) answerWithPreconditions).Result;
			Assert.AreSame(concept1, resultComparison.LeftValue);
			Assert.AreSame(concept3, resultComparison.RightValue);
			Assert.AreSame(ComparisonSigns.IsEqualTo, resultComparison.ComparisonSign);
			Assert.AreEqual(2, answerWithPreconditions.Explanation.Statements.Count);
			Assert.IsTrue(answerWithPreconditions.Explanation.Statements.Contains(initialComparison));
			Assert.IsTrue(answerWithPreconditions.Explanation.Statements.Contains(preconditionComparison));

			Assert.AreSame(initialComparison, semanticNetwork.Statements.Single());
		}
	}
}
