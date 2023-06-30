using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Set.Questions;

namespace AabSemantics.Modules.Set.Tests.Questions
{
	[TestFixture]
	public class QuestionTest
	{
		[Test]
		[TestCaseSource(nameof(CreateQuestionsArgumentNullException))]
		public void GivenNullArguments_WhenCreateQuestions_ThenFail(Func<IQuestion> constructor)
		{
			Assert.Throws<ArgumentNullException>(() => constructor());
		}

		private static IEnumerable<object[]> CreateQuestionsArgumentNullException()
		{
			IConcept concept = "test".CreateConcept();

			yield return new object[] { new Func<IQuestion>(() => new DescribeSubjectAreaQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new EnumerateContainersQuestion(null)) };
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
			yield return new object[] { new Func<IQuestion>(() => new IsSignQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsSubjectAreaQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new IsSubjectAreaQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new IsValueQuestion(null)) };
			yield return new object[] { new Func<IQuestion>(() => new SignValueQuestion(null, concept)) };
			yield return new object[] { new Func<IQuestion>(() => new SignValueQuestion(concept, null)) };
			yield return new object[] { new Func<IQuestion>(() => new WhatQuestion(null)) };
		}
	}
}
