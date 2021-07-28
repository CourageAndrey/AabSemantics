using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Processors
{
	[TestFixture]
	public class QuestionProcessorTest
	{
		[Test]
		public void ExplainPreconditions()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var concept1 = new Concept();
			concept1.Attributes.Add(IsValueAttribute.Value);
			var concept2 = new Concept();
			concept2.Attributes.Add(IsValueAttribute.Value);
			var concept3 = new Concept();
			concept3.Attributes.Add(IsValueAttribute.Value);
			knowledgeBase.Concepts.Add(concept1);
			knowledgeBase.Concepts.Add(concept2);
			knowledgeBase.Concepts.Add(concept3);

			var initialComparison = new ComparisonStatement(concept1, concept2, ComparisonSigns.IsEqualTo);
			knowledgeBase.Statements.Add(initialComparison);

			var preconditionComparison = new ComparisonStatement(concept2, concept3, ComparisonSigns.IsEqualTo);
			// ... and do not add it to knowledge base

			var question = new ComparisonQuestion(concept1, concept3);

			var questionWithPreconditions = new ComparisonQuestion(concept1, concept3);
			questionWithPreconditions.Preconditions.Add(preconditionComparison);

			// act
			var answerWithoutPreconditions = question.Ask(knowledgeBase.Context);
			var answerWithPreconditions = questionWithPreconditions.Ask(knowledgeBase.Context);

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

			Assert.AreSame(initialComparison, knowledgeBase.Statements.Single());
		}
	}
}
