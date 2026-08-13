using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Processes.Tests.Concurrency
{
	/// <summary>
	/// Thread safety of the processes module against a single shared semantic network. Answering a
	/// sequence question expands every recorded sign into its consequences and composes signs across
	/// shared processes, so one call already does a lot of concurrent nested work.
	/// </summary>
	[TestFixture]
	public class ConcurrentProcessesTest
	{
		private const Int32 CallCount = 200;

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskSequenceQuestionConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateProcessesTestData();
			var expected = (StatementsAnswer<ProcessesStatement>) new ProcessesQuestion(semanticNetwork.ProcessA, semanticNetwork.ProcessB)
				.Ask(semanticNetwork.SemanticNetwork.Context);

			// act
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, () => (StatementsAnswer<ProcessesStatement>) new ProcessesQuestion(semanticNetwork.ProcessA, semanticNetwork.ProcessB)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert: every concurrent answer reports the same signs as the sequential one
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(answer => answer.IsEmpty == expected.IsEmpty), Is.True);
			Assert.That(answers.Select(answer => answer.Result.Count).Distinct(), Is.EqualTo(new[] { expected.Result.Count }));
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskSequenceQuestionInBothDirectionsConcurrently_ThenEveryAnswerIsCorrect()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateProcessesTestData();
			var forward = ((StatementsAnswer<ProcessesStatement>) new ProcessesQuestion(semanticNetwork.ProcessA, semanticNetwork.ProcessB)
				.Ask(semanticNetwork.SemanticNetwork.Context)).Result.Count;
			var backward = ((StatementsAnswer<ProcessesStatement>) new ProcessesQuestion(semanticNetwork.ProcessB, semanticNetwork.ProcessA)
				.Ask(semanticNetwork.SemanticNetwork.Context)).Result.Count;

			// act: asking in both operand orders at once exercises the operand-swapping path
			var answers = ConcurrencyHelper.RunConcurrently(CallCount, index =>
			{
				bool straight = index % 2 == 0;
				var answer = (StatementsAnswer<ProcessesStatement>) new ProcessesQuestion(
						straight ? semanticNetwork.ProcessA : semanticNetwork.ProcessB,
						straight ? semanticNetwork.ProcessB : semanticNetwork.ProcessA)
					.Ask(semanticNetwork.SemanticNetwork.Context);
				return new { Expected = straight ? forward : backward, Actual = answer.Result.Count };
			});

			// assert
			Assert.That(answers.Count, Is.EqualTo(CallCount));
			Assert.That(answers.All(pair => pair.Actual == pair.Expected), Is.True);
		}

		[Test]
		public void GivenSharedSemanticNetwork_WhenAskSequenceQuestionConcurrently_ThenNoQuestionContextLeaks()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateProcessesTestData();

			// act
			ConcurrencyHelper.RunConcurrently(CallCount, () => new ProcessesQuestion(semanticNetwork.ProcessA, semanticNetwork.ProcessB)
				.Ask(semanticNetwork.SemanticNetwork.Context));

			// assert
			Assert.That(semanticNetwork.SemanticNetwork.Context.Children.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenProcessesStatements_WhenCheckForContradictionsConcurrently_ThenEveryResultIsTheSame()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default).CreateProcessesTestData();
			var statements = semanticNetwork.SemanticNetwork.Statements.OfType<ProcessesStatement>().ToList();
			int expected = statements.CheckForContradictions().Count;

			// act: every call builds its own inference matrix over the shared statements
			var counts = ConcurrencyHelper.RunConcurrently(50, () => statements.CheckForContradictions().Count);

			// assert
			Assert.That(counts.Distinct(), Is.EqualTo(new[] { expected }));
		}
	}
}
