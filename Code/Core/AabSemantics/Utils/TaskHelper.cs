using System;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	public static class TaskHelper
	{
		public static T Await<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static void Await(this Task task)
		{
			task.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static T AwaitDetached<T>(Func<Task<T>> asyncOperation)
		{
			return Task.Run(asyncOperation).Await();
		}

		public static void AwaitDetached(Func<Task> asyncOperation)
		{
			Task.Run(asyncOperation).Await();
		}
	}
}
