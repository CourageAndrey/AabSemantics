using System;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	/// <summary>
	/// Bridges synchronous and asynchronous code in both directions. The <c>Detached</c> variants
	/// start the work on the thread pool first, which is what keeps them from deadlocking when
	/// called from a thread with a synchronization context, such as the WPF UI thread;
	/// <see cref="FromSynchronous{T}"/> goes the other way, giving work that has nothing to await
	/// an asynchronous signature.
	/// </summary>
	public static class TaskHelper
	{
		/// <summary>Blocks until the task completes and returns its result.</summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="task">Task to wait for.</param>
		/// <returns>The task's result.</returns>
		public static T Await<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>Blocks until the task completes.</summary>
		/// <param name="task">Task to wait for.</param>
		public static void Await(this Task task)
		{
			task.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>Runs an asynchronous operation on the thread pool and blocks for its result.</summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="asyncOperation">Operation to run.</param>
		/// <returns>The operation's result.</returns>
		public static T AwaitDetached<T>(Func<Task<T>> asyncOperation)
		{
			return Task.Run(asyncOperation).Await();
		}

		/// <summary>Runs an asynchronous operation on the thread pool and blocks until it finishes.</summary>
		/// <param name="asyncOperation">Operation to run.</param>
		public static void AwaitDetached(Func<Task> asyncOperation)
		{
			Task.Run(asyncOperation).Await();
		}

		/// <summary>
		/// Runs an operation that has nothing to await on the calling thread and reports its outcome
		/// through a completed task. Failures reach the caller the same way they would if the work
		/// had really been asynchronous, instead of being thrown before the task is returned.
		/// </summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="operation">Operation to run.</param>
		/// <returns>A completed or faulted task.</returns>
		public static Task<T> FromSynchronous<T>(Func<T> operation)
		{
			try
			{
				return Task.FromResult(operation());
			}
			catch (Exception error)
			{
				return Task.FromException<T>(error);
			}
		}

		/// <summary>
		/// Runs an operation that has nothing to await and produces no result on the calling thread,
		/// and reports its outcome through a completed task.
		/// </summary>
		/// <param name="operation">Operation to run.</param>
		/// <returns>A completed or faulted task.</returns>
		public static Task FromSynchronous(Action operation)
		{
			try
			{
				operation();
				return Task.CompletedTask;
			}
			catch (Exception error)
			{
				return Task.FromException(error);
			}
		}
	}
}
