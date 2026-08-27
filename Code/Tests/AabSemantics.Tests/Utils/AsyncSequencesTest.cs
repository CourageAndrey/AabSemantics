using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.Tests.Utils
{
	[TestFixture]
	public class AsyncSequencesTest
	{
		private static readonly int[] _empty = new int[0];
		private static readonly int[] _items = { 1, 2, 3 };

		private static async Task CheckMethodWorks<T>(Task<T> task, Action<T> assert)
		{
			// the operators enumerate in-memory sequences, so there is nothing left to await
			Assert.That(task.IsCompleted, Is.True);

			assert(await task);
		}

		private static void CheckMethodFails<ErrorT>(Task task)
			where ErrorT : Exception
		{
			// failures are reported through the task, not thrown at the call site
			Assert.That(task.IsFaulted, Is.True);
			Assert.That(task.Exception.InnerException, Is.InstanceOf<ErrorT>());
		}

		[Test]
		public async Task GivenSequence_CheckFirstAsync()
		{
			CheckMethodFails<InvalidOperationException>(_empty.FirstAsync());
			await CheckMethodWorks(_items.FirstAsync(), result => Assert.That(result, Is.EqualTo(1)));

			await CheckMethodWorks(_items.FirstAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks(_items.FirstAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			CheckMethodFails<InvalidOperationException>(_empty.FirstAsync(i => i == 1));
			CheckMethodFails<InvalidOperationException>(_items.FirstAsync(i => i == 0));
		}

		[Test]
		public async Task GivenSequence_CheckFirstOrDefaultAsync()
		{
			await CheckMethodWorks(_empty.FirstOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks(_items.FirstOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(1)));

			await CheckMethodWorks(_items.FirstOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks(_items.FirstOrDefaultAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks(_empty.FirstOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks(_items.FirstOrDefaultAsync(i => i == 0), result => Assert.That(result, Is.EqualTo(0)));
		}

		[Test]
		public async Task GivenSequence_CheckLastAsync()
		{
			CheckMethodFails<InvalidOperationException>(_empty.LastAsync());
			await CheckMethodWorks(_items.LastAsync(), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks(_items.LastAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks(_items.LastAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			CheckMethodFails<InvalidOperationException>(_empty.LastAsync(i => i == 1));
			CheckMethodFails<InvalidOperationException>(_items.LastAsync(i => i == 0));
		}

		[Test]
		public async Task GivenSequence_CheckLastOrDefaultAsync()
		{
			await CheckMethodWorks(_empty.LastOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks(_items.LastOrDefaultAsync(), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks(_items.LastOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(1)));
			await CheckMethodWorks(_items.LastOrDefaultAsync(i => i == 3), result => Assert.That(result, Is.EqualTo(3)));

			await CheckMethodWorks(_empty.LastOrDefaultAsync(i => i == 1), result => Assert.That(result, Is.EqualTo(0)));
			await CheckMethodWorks(_items.LastOrDefaultAsync(i => i == 0), result => Assert.That(result, Is.EqualTo(0)));
		}

		[Test]
		public async Task GivenSequence_CheckAnyAsync()
		{
			await CheckMethodWorks(_empty.AnyAsync(), result => Assert.That(result, Is.False));
			await CheckMethodWorks(_items.AnyAsync(), result => Assert.That(result, Is.True));

			await CheckMethodWorks(_items.AnyAsync(i => i == 0), result => Assert.That(result, Is.False));
			await CheckMethodWorks(_items.AnyAsync(i => i == 2), result => Assert.That(result, Is.True));
		}

		[Test]
		public async Task GivenSequence_CheckAllAsync()
		{
			await CheckMethodWorks(_empty.AllAsync(i => i < 3), result => Assert.That(result, Is.True));
			await CheckMethodWorks(_items.AllAsync(i => i == 3), result => Assert.That(result, Is.False));
			await CheckMethodWorks(_items.AllAsync(i => i < 4), result => Assert.That(result, Is.True));
		}

		[Test]
		public async Task GivenSequence_CheckToArrayAsync()
		{
			await CheckMethodWorks(_empty.ToArrayAsync(), result =>
			{
				Assert.That(result.Length, Is.EqualTo(0));
			});
			await CheckMethodWorks(_items.ToArrayAsync(), result =>
			{
				Assert.That(result.Length, Is.EqualTo(3));
				Assert.That(result[0], Is.EqualTo(1));
				Assert.That(result[1], Is.EqualTo(2));
				Assert.That(result[2], Is.EqualTo(3));
			});
		}

		[Test]
		public async Task GivenSequence_CheckToListAsync()
		{
			await CheckMethodWorks(_empty.ToListAsync(), result =>
			{
				Assert.That(result.Count, Is.EqualTo(0));
			});
			await CheckMethodWorks(_items.ToListAsync(), result =>
			{
				Assert.That(result.Count, Is.EqualTo(3));
				Assert.That(result[0], Is.EqualTo(1));
				Assert.That(result[1], Is.EqualTo(2));
				Assert.That(result[2], Is.EqualTo(3));
			});
		}

		[Test]
		public void GivenSequence_WhenEnumerate_ThenDoNotLeaveCallingThread()
		{
			// arrange
			int callingThread = Thread.CurrentThread.ManagedThreadId;
			var enumerationThreads = new List<int>();
			var sequence = _items.Select(item =>
			{
				enumerationThreads.Add(Thread.CurrentThread.ManagedThreadId);
				return item;
			});

			// act
			var task = sequence.ToListAsync();

			// assert
			Assert.That(task.IsCompleted, Is.True);
			Assert.That(enumerationThreads, Is.EqualTo(new[] { callingThread, callingThread, callingThread }));
		}

		[Test]
		public void GivenCancelledToken_WhenEnumerate_ThenDoNotStartAtAll()
		{
			// arrange
			bool enumerated = false;
			var sequence = _items.Select(item =>
			{
				enumerated = true;
				return item;
			});

			using (var cancellation = new CancellationTokenSource())
			{
				cancellation.Cancel();

				// act
				var task = sequence.ToListAsync(cancellation.Token);

				// assert
				Assert.That(task.IsCanceled, Is.True);
				Assert.That(enumerated, Is.False);
			}
		}

		[Test]
		public void GivenTokenCancelledDuringEnumeration_WhenEnumerate_ThenStopAtOnce()
		{
			// arrange
			using (var cancellation = new CancellationTokenSource())
			{
				var enumeratedItems = new List<int>();
				var sequence = _items.Select(item =>
				{
					enumeratedItems.Add(item);
					if (item == 2)
					{
						cancellation.Cancel();
					}
					return item;
				});

				// act
				var task = sequence.ToListAsync(cancellation.Token);

				// assert
				Assert.That(task.IsCanceled, Is.True);
				Assert.That(enumeratedItems, Is.EqualTo(new[] { 1, 2 }));
			}
		}

		[Test]
		public void GivenFailingPredicate_WhenEnumerate_ThenReportErrorThroughTask()
		{
			// act
			var task = _items.AllAsync(_ => throw new NotSupportedException("boom"));

			// assert
			CheckMethodFails<NotSupportedException>(task);
		}
	}
}
