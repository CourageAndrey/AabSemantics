using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Questions;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Mathematics.Tests.Concurrency
{
	/// <summary>
	/// Thread safety of the mathematics module against a single shared semantic network. Comparison
	/// questions recurse through the values' hierarchies, so several nested questions are answered
	/// concurrently even within one call.
	/// </summary>
	[TestFixture]
	public class ConcurrentMathematicsTest
	{
		private const Int32 CallCount = 200;

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskComparisonQuestionConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateMathematicsTestData();
			var expected = (StatementAnswer) new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number2)
				.Ask(semanticNetwork.SemanticNetwork.Context);

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (StatementAnswer) new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number2)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert: every concurrent answer matches the sequential one
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.IsEmpty == expected.IsEmpty), Is.True);
			Assert.That(answers.Select(answer => ((ComparisonStatement) answer.Result).ComparisonSign).Distinct().Count(), Is.EqualTo(1));
			Assert.That(((ComparisonStatement) answers.First().Result).ComparisonSign, Is.EqualTo(ComparisonSigns.IsLessThan));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskDifferentComparisonsConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateMathematicsTestData();
			var pairs = new[]
			{
				new { Left = semanticNetwork.Number0, Right = semanticNetwork.Number1, Sign = ComparisonSigns.IsLessThan },
				new { Left = semanticNetwork.Number1, Right = semanticNetwork.Number2, Sign = ComparisonSigns.IsLessThan },
				new { Left = semanticNetwork.Number3, Right = semanticNetwork.Number2, Sign = ComparisonSigns.IsGreaterThan },
				new { Left = semanticNetwork.Number4, Right = semanticNetwork.Number3, Sign = ComparisonSigns.IsGreaterThan },
				new { Left = semanticNetwork.Number0, Right = semanticNetwork.NumberZero, Sign = ComparisonSigns.IsEqualTo },
			};

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, index =>
			{
				var pair = pairs[index % pairs.Length];
				var answer = (StatementAnswer) new ComparisonQuestion(pair.Left, pair.Right).Ask(semanticNetwork.SemanticNetwork.Context);
				return new { Expected = pair.Sign, Actual = ((ComparisonStatement) answer.Result).ComparisonSign };
			});

			// assert
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(pair => pair.Actual == pair.Expected), Is.True);
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskComparisonQuestionConcurrently_ThenNoQuestionContextLeaks()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateMathematicsTestData();

			// act
			ConcurrencyHelper.RunConcurrently(CallCount, () => new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number4)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert
			Assert.That(semanticNetwork.SemanticNetwork.Context.Children.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenComparisonStatements_WhenCheckForContradictionsConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateMathematicsTestData();
			var statements = semanticNetwork.SemanticNetwork.Statements.OfType<ComparisonStatement>().ToList();
			int expected = statements.CheckForContradictions().Count;

			// act: every call builds its own inference matrix over the shared statements
			var counts = ConcurrencyHelper.RunConcurrently(50, () => statements.CheckForContradictions().Count);

			// assert
			Assert.That(counts.Distinct(), Is.EqualTo(new[] { expected }));
		}

	}
}
