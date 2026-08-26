using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class QuestionCancellationTest
	{
		#region The token reaches the question context

		[Test]
		public async Task GivenToken_WhenAsk_ThenQuestionContextCarriesIt()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				var question = new TokenCapturingQuestion();

				// act
				await question.AskAsync(semanticNetwork.Context, null, tokenSource.Token);

				// assert
				Assert.That(question.CapturedToken, Is.EqualTo(tokenSource.Token));
			}
		}

		[Test]
		public void GivenNoToken_WhenAsk_ThenQuestionContextCarriesNone()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var question = new TokenCapturingQuestion();

			// act
			question.Ask(semanticNetwork.Context);

			// assert
			Assert.That(question.CapturedToken.CanBeCanceled, Is.False);
		}

		[Test]
		public async Task GivenNestedQuestionAskedWithoutToken_WhenAsk_ThenEnclosingQuestionsTokenIsInherited()
		{
			// arrange: a nested question asked through the plain overload must not escape the
			// cancellation of the question that asked it
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				var nested = new TokenCapturingQuestion();

				// act
				await new NestingQuestion(nested).AskAsync(semanticNetwork.Context, null, tokenSource.Token);

				// assert
				Assert.That(nested.CapturedToken, Is.EqualTo(tokenSource.Token));
			}
		}

		#endregion

		#region Cancelling before the work starts

		[Test]
		public void GivenAlreadyCancelledToken_WhenAskAsync_ThenThrowBeforeProcessing()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var question = new TokenCapturingQuestion();

				// act & assert
				Assert.That(
					async () => await question.AskAsync(semanticNetwork.Context, null, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
				Assert.That(question.WasProcessed, Is.False);
			}
		}

		[Test]
		public void GivenAlreadyCancelledToken_WhenAsk_ThenThrow()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();
				var question = new TokenCapturingQuestion();

				// act & assert: the blocking wrapper reports cancellation just as the asynchronous one does
				Assert.That(
					() => question.Ask(semanticNetwork.Context, null, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		#endregion

		#region Cancelling work already under way

		[Test]
		public void GivenTokenCancelledWhileFiltering_WhenAsk_ThenThrowAndStopEnumerating()
		{
			// arrange: a network big enough that filtering it takes many steps
			var semanticNetwork = CreateHierarchy(1000, out var root, out var leaf);
			using (var tokenSource = new CancellationTokenSource())
			{
				// the filter cancels itself once it has seen a few statements
				int seen = 0;
				var question = new FilteringQuestion(statement =>
				{
					if (++seen == 10)
					{
						tokenSource.Cancel();
					}
					return true;
				});

				// act & assert
				Assert.That(
					async () => await question.AskAsync(semanticNetwork.Context, null, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());

				// the enumeration stopped rather than running to the end of the network
				Assert.That(seen, Is.LessThan(999));
			}
		}

		[Test]
		public void GivenTokenCancelledWhileWalkingHierarchy_WhenFindPath_ThenThrow()
		{
			// arrange: FindPathAsync widens its search generation by generation, which is the loop
			// a cancelled token has to break out of
			var semanticNetwork = CreateHierarchy(1000, out var root, out var leaf);

			using (var tokenSource = new CancellationTokenSource())
			{
				var statements = CancelAfter(semanticNetwork.Statements.Enumerate().ToList(), 10, tokenSource);

				// act & assert
				Assert.That(
					async () => await statements.FindPathAsync(typeof(IsStatement), root, leaf, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		[Test]
		public void GivenTokenCancelledWhileWalkingHierarchy_WhenGetParentsAllLevels_ThenThrow()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(1000, out _, out var leaf);

			using (var tokenSource = new CancellationTokenSource())
			{
				var statements = CancelAfter(semanticNetwork.Statements.Enumerate().ToList(), 10, tokenSource);

				// act & assert
				Assert.That(
					async () => await statements.GetParentsAllLevelsAsync<IConcept, IsStatement>(leaf, null, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		[Test]
		public void GivenTokenCancelledWhileAskingTransitives_WhenAsk_ThenThrow()
		{
			// arrange: a long "is a" chain, so answering recurses through a nested question per link.
			// Answering it uncancelled takes ~200 ms, which leaves the timer below a wide margin.
			var semanticNetwork = CreateHierarchy(1000, out var root, out var leaf);

			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.CancelAfter(TimeSpan.FromMilliseconds(1));

				// act & assert
				Assert.That(
					async () => await new IsQuestion(leaf, root).AskAsync(semanticNetwork.Context, null, tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		[Test]
		public void GivenContextWithCancelledToken_WhenCheckStatementProcesses_ThenThrow()
		{
			// arrange: CheckStatementQuestion proves hierarchical statements through FindPathAsync.
			// Processing is entered directly, because AskAsync would reject the cancelled token
			// before the processor ever ran — and it is the processor that has to pass the token on.
			var semanticNetwork = CreateHierarchy(10, out var root, out var leaf);
			var question = new CheckStatementQuestion(new IsStatement(null, root, leaf));

			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();

				using (var context = semanticNetwork.Context.CreateQuestionContext(question, null, tokenSource.Token))
				{
					// act & assert
					Assert.That(
						async () => await question.ProcessAsync(context),
						Throws.InstanceOf<OperationCanceledException>());
				}
			}
		}

		[Test]
		public void GivenTokenGivenToBuilder_WhenAskThroughFluentSyntax_ThenItReachesTheQuestion()
		{
			// arrange: the fluent syntax takes the token once, when the builder is created
			var semanticNetwork = CreateHierarchy(1000, out var root, out var leaf);

			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.CancelAfter(TimeSpan.FromMilliseconds(1));

				// act & assert
				Assert.That(
					async () => await semanticNetwork.Ask(tokenSource.Token).IfIsAsync(leaf, root),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		#endregion

		#region Not cancelling changes nothing

		[Test]
		public async Task GivenTokenNeverCancelled_WhenAsk_ThenAnswerIsTheSameAsWithoutToken()
		{
			// arrange
			var semanticNetwork = CreateHierarchy(10, out var root, out var leaf);

			using (var tokenSource = new CancellationTokenSource())
			{
				// act
				var withToken = await new IsQuestion(leaf, root).AskAsync(semanticNetwork.Context, null, tokenSource.Token);
				var withoutToken = await new IsQuestion(leaf, root).AskAsync(semanticNetwork.Context);

				// assert
				Assert.That(((BooleanAnswer) withToken).Result, Is.True);
				Assert.That(((BooleanAnswer) withToken).Result, Is.EqualTo(((BooleanAnswer) withoutToken).Result));
			}
		}

		#endregion

		#region Helpers

		private static SemanticNetwork CreateHierarchy(Int32 length, out IConcept root, out IConcept leaf)
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			var concepts = System.Linq.Enumerable.Range(0, length)
				.Select(index => $"concept-{index}".CreateConceptByName())
				.ToList();
			foreach (var concept in concepts)
			{
				semanticNetwork.Concepts.Add(concept);
			}
			for (int index = 0; index < length - 1; index++)
			{
				semanticNetwork.DeclareThat(concepts[index]).IsAncestorOf(concepts[index + 1]);
			}

			root = concepts.First();
			leaf = concepts.Last();
			return semanticNetwork;
		}

		private static IEnumerable<T> CancelAfter<T>(IEnumerable<T> sequence, Int32 afterItems, CancellationTokenSource tokenSource)
		{
			int yielded = 0;
			foreach (var item in sequence)
			{
				if (++yielded == afterItems)
				{
					tokenSource.Cancel();
				}
				yield return item;
			}
		}

		private class TokenCapturingQuestion : Question
		{
			public CancellationToken CapturedToken
			{ get; private set; }

			public Boolean WasProcessed
			{ get; private set; }

			public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
			{
				CapturedToken = context.CancellationToken;
				WasProcessed = true;
				return await Task.FromResult(Answer.CreateUnknown());
			}
		}

		private class NestingQuestion : Question
		{
			private readonly IQuestion _nested;

			public NestingQuestion(IQuestion nested)
			{
				_nested = nested;
			}

			public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
			{
				return await _nested.AskAsync(context);
			}
		}

		private class FilteringQuestion : Question
		{
			private readonly Func<IsStatement, Boolean> _match;

			public FilteringQuestion(Func<IsStatement, Boolean> match)
			{
				_match = match;
			}

			public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
			{
				return await context
					.From<FilteringQuestion, IsStatement>()
					.Where(statement => _match(statement))
					.SelectStatementsAsync();
			}
		}

		#endregion
	}
}
