using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	public static class AsyncSequences
	{
		public static async Task<T> FirstAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.First(predicate))
				: Task.Run(() => sequence.First())).ConfigureAwait(false);
		}

		public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.FirstOrDefault(predicate))
				: Task.Run(() => sequence.FirstOrDefault())).ConfigureAwait(false);
		}

		public static async Task<T> LastAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.Last(predicate))
				: Task.Run(() => sequence.Last())).ConfigureAwait(false);
		}

		public static async Task<T> LastOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.LastOrDefault(predicate))
				: Task.Run(() => sequence.LastOrDefault())).ConfigureAwait(false);
		}

		public static async Task<Boolean> AnyAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.Any(predicate))
				: Task.Run(() => sequence.Any())).ConfigureAwait(false);
		}

		public static async Task<Boolean> AllAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate)
		{
			return await Task.Run(() => sequence.All(predicate)).ConfigureAwait(false);
		}

		public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<T> sequence)
		{
			return await Task.Run(() => sequence.ToArray()).ConfigureAwait(false);
		}

		public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> sequence)
		{
			return await Task.Run(() => sequence.ToList()).ConfigureAwait(false);
		}
	}
}
