using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Utils;

namespace AabSemantics.TestCore
{
	[TestFixture]
	public class TaskHelperTest
	{
		[Test]
		public void Await_TResult_CompletesSuccessfully()
		{
			// arrange
			var task = Task.FromResult(123);

			// act
			int result = task.Await();

			// assert
			Assert.That(result, Is.EqualTo(123));
		}

		[Test]
		public void Await_Void_CompletesSuccessfully()
		{
			// arrange
			bool completed = false;

			var task = Task.Run(async () =>
			{
				await Task.Delay(10).ConfigureAwait(false);
				completed = true;
			});

			// act
			task.Await();

			// assert
			Assert.That(completed, Is.True);
		}

		[Test]
		public void Await_TResult_PropagatesException()
		{
			// arrange
			Task<int> Faulty()
			{
				return Task.Run(new Func<int>(() => throw new NotSupportedException("boom")));
			}

			// act
			var error = Assert.Throws<NotSupportedException>(() => Faulty().Await());

			// assert
			Assert.That(error.Message, Is.EqualTo("boom"));
		}

		[Test]
		public void Await_Void_PropagatesException()
		{
			// arrange
			Task Faulty()
			{
				return Task.Run(() => throw new NotSupportedException("boom"));
			}

			// act
			var error = Assert.Throws<NotSupportedException>(() => Faulty().Await());

			// assert
			Assert.That(error.Message, Is.EqualTo("boom"));
		}

		[Test]
		public void Await_DoesNotDeadlock_WithSingleThreadSynchronizationContext_WhenTaskDoesNotCaptureContext()
		{
			var currentSynchronizationContext = SynchronizationContext.Current;
			try
			{
				var testSynchronizationContext = new SingleThreadSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(testSynchronizationContext);

				// This async method does not capture the sync context
				async Task<int> NoCaptureAsync()
				{
					await Task.Delay(20).ConfigureAwait(false);
					return 7;
				}

				var task = NoCaptureAsync();

				// Block on the same thread; should not deadlock because the task doesn't try to resume on this context
				int result = task.Await();
				Assert.That(result, Is.EqualTo(7));
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(currentSynchronizationContext);
			}
		}

		[Test]
		public void Await_Deadlocks_WithSingleThreadSynchronizationContext_WhenTaskCapturesContext()
		{
			var currentSynchronizationContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext(new SingleThreadSynchronizationContext());

				// This async method captures the sync context, exactly like the library code used to do.
				async Task<int> CaptureAsync()
				{
					await Task.Delay(20);
					return 7;
				}

				var task = CaptureAsync();

				// Await() can not save such a task: its continuation is posted to the context
				// which this very thread is about to block. That is why AwaitDetached exists.
				Assert.That(task.Wait(TimeSpan.FromSeconds(2)), Is.False, "the task is expected to be unable to complete while its context is blocked");
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(currentSynchronizationContext);
			}
		}

		[Test]
		public void AwaitDetached_DoesNotDeadlock_WithSingleThreadSynchronizationContext_WhenTaskCapturesContext()
		{
			var currentSynchronizationContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext(new SingleThreadSynchronizationContext());

				// The very same context-capturing operation which deadlocks under Await().
				async Task<int> CaptureAsync()
				{
					await Task.Delay(20);
					return 7;
				}

				// act: started on a thread pool thread, so there is no context to capture at all
				int result = TaskHelper.AwaitDetached(() => CaptureAsync());

				// assert
				Assert.That(result, Is.EqualTo(7));
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(currentSynchronizationContext);
			}
		}

		[Test]
		public void AwaitDetached_Void_DoesNotDeadlock_WithSingleThreadSynchronizationContext()
		{
			var currentSynchronizationContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext(new SingleThreadSynchronizationContext());

				bool executed = false;
				async Task CaptureAsync()
				{
					await Task.Delay(20);
					executed = true;
				}

				// act
				TaskHelper.AwaitDetached(() => CaptureAsync());

				// assert
				Assert.That(executed, Is.True);
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext(currentSynchronizationContext);
			}
		}

		private sealed class SingleThreadSynchronizationContext : SynchronizationContext
		{
			private readonly BlockingCollection<(SendOrPostCallback d, object state)> _queue = new BlockingCollection<(SendOrPostCallback, object)>();
			private readonly Thread _thread;
			private volatile bool _done;

			public SingleThreadSynchronizationContext()
			{
				_thread = Thread.CurrentThread;
			}

			public override void Post(SendOrPostCallback callback, object state)
			{
				_queue.Add((callback, state));
			}

			public override void Send(SendOrPostCallback callback, object state)
			{
				if (Thread.CurrentThread == _thread)
				{
					callback(state);
				}
				else
				{
					using (var syncEvent = new ManualResetEvent(false))
					{
						Post(s =>
						{
							try
							{
								callback(s);
							}
							finally
							{
								syncEvent.Set();
							}
						}, state);
						syncEvent.WaitOne();
					}
				}
			}

			public void PumpOnce(int timeoutMs = 50)
			{
				if (_queue.TryTake(out var work, timeoutMs))
				{
					work.d(work.state);
				}
			}

			public void Complete() => _done = true;

			public void RunLoopUntil(Task task)
			{
				while (!_done && !task.IsCompleted)
				{
					PumpOnce();
				}
			}
		}
	}
}


