using System;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class ConsistencyCancellationTest
	{
		[Test]
		public void GivenCancelledToken_WhenCheckConsistency_ThenThrowBeforeChecking()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				var comparisons = new Int32[1];
				semanticNetwork.Statements.Add(new CancellingStatement(tokenSource, comparisons, Int32.MaxValue));

				tokenSource.Cancel();

				// act & assert
				Assert.That(
					async () => await semanticNetwork.CheckConsistencyAsync(tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());

				Assert.That(comparisons[0], Is.EqualTo(0));
			}
		}

		[Test]
		public void GivenTokenCancelledWhileCheckingDuplicates_WhenCheckConsistency_ThenThrowAndStopComparing()
		{
			// arrange: the duplicates check compares every statement with every other one, which is
			// the loop a cancelled token has to break out of
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				var comparisons = new Int32[1];
				for (Int32 i = 0; i < 10; i++)
				{
					semanticNetwork.Statements.Add(new CancellingStatement(tokenSource, comparisons, 3));
				}

				// act & assert
				Assert.That(
					async () => await semanticNetwork.CheckConsistencyAsync(tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());

				// the comparison which cancelled the token was the last one made
				Assert.That(comparisons[0], Is.EqualTo(3));
			}
		}

		[Test]
		public void GivenCancelledToken_WhenDescribeRules_ThenThrow()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();

				// act & assert
				Assert.That(
					async () => await semanticNetwork.DescribeRulesAsync(tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		[Test]
		public void GivenCancelledToken_WhenCheckConsistencyByBlockingCall_ThenThrow()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			using (var tokenSource = new CancellationTokenSource())
			{
				tokenSource.Cancel();

				// act & assert: the blocking wrapper reports cancellation just as the asynchronous one does
				Assert.That(
					() => semanticNetwork.CheckConsistency(tokenSource.Token),
					Throws.InstanceOf<OperationCanceledException>());
			}
		}

		/// <summary>Cancels the token once it has been compared with the given number of statements.</summary>
		private class CancellingStatement : TestStatement
		{
			private readonly CancellationTokenSource _cancellation;
			private readonly Int32[] _comparisons;
			private readonly Int32 _cancelAfter;

			public CancellingStatement(CancellationTokenSource cancellation, Int32[] comparisons, Int32 cancelAfter)
			{
				_cancellation = cancellation;
				_comparisons = comparisons;
				_cancelAfter = cancelAfter;
			}

			public override Boolean Equals(TestStatement other)
			{
				if (++_comparisons[0] == _cancelAfter)
				{
					_cancellation.Cancel();
				}

				return ReferenceEquals(this, other);
			}
		}
	}
}
