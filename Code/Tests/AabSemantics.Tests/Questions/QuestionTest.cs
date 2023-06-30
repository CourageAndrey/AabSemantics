using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class QuestionTest
	{
		[Test]
		public void GivenPreconditions_WhenBeingAsked_ThenQuestionIsProcessedTakenThemIntoAccount()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = ConceptCreationHelper.CreateConcept();
			concept1.WithAttribute(IsValueAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateConcept();
			concept2.WithAttribute(IsValueAttribute.Value);
			var concept3 = ConceptCreationHelper.CreateConcept();
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

		[Test]
		[TestCaseSource(nameof(CreateQuestionsArgumentNullException))]
		public void GivenNullArguments_WhenCreateQuestions_ThenFail(Func<IQuestion> constructor)
		{
			Assert.Throws<ArgumentNullException>(() => constructor());
		}

		private static IEnumerable<object[]> CreateQuestionsArgumentNullException()
		{
			IConcept concept = "test".CreateConcept();

			yield return new object[] { new Func<IQuestion>(() => new CheckStatementQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new ComparisonQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new ComparisonQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new DescribeSubjectAreaQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumerateAncestorsQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumerateContainersQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumerateDescendantsQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumeratePartsQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumerateSignsQuestion(null, false)) };
			yield return new object[] { new Func<IQuestion>(() => new FindSubjectAreaQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new GetCommonQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new GetCommonQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new GetDifferencesQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new GetDifferencesQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new HasSignQuestion(null, concept, false)) };
			yield return new object[] { new Func<IQuestion>(() => new HasSignQuestion(concept, null, false)) };
			yield return new object[] { new Func<IQuestion>(() => new HasSignsQuestion(null, false)) };
			yield return new object[] { new Func<IQuestion>(() => new IsPartOfQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new IsPartOfQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new IsQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsSignQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsSubjectAreaQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new IsSubjectAreaQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsValueQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new ProcessesQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new ProcessesQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new SignValueQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new SignValueQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new WhatQuestion(null)) };
		}

		[Test]
		public void GivenAllParameterSet_WhenCreateChildAnswer_ThenSucceed()
		{
			// arrange
			var concept = "test".CreateConcept();
			var question = new HasSignsQuestion(concept, false);
			var answer = Answer.CreateUnknown();
			var transitives = Array.Empty<IStatement>();

			// act
			var child = new ChildAnswer(question, answer, transitives);

			// assert
			Assert.AreSame(question, child.Question);
			Assert.AreSame(answer, child.Answer);
			Assert.AreSame(transitives, child.TransitiveStatements);
		}
	}
}
