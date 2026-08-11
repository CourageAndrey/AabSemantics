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
	}
}
