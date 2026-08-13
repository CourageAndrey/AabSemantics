using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	/// <summary>
	/// Asynchronous wrappers over the LINQ operators used during inference. Each one offloads
	/// the enumeration to the thread pool, so a long traversal does not block the caller.
	/// </summary>
	public static class AsyncSequences
	{
		/// <summary>Returns the first matching item.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the first item of the sequence.</param>
		/// <returns>The first match.</returns>
		/// <exception cref="InvalidOperationException">Nothing matched.</exception>
		public static async Task<T> FirstAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.First(predicate))
				: Task.Run(() => sequence.First())).ConfigureAwait(false);
		}

		/// <summary>Returns the first matching item, or the type's default.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the first item of the sequence.</param>
		/// <returns>The first match, or the type's default when nothing matched.</returns>
		public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.FirstOrDefault(predicate))
				: Task.Run(() => sequence.FirstOrDefault())).ConfigureAwait(false);
		}

		/// <summary>Returns the last matching item.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the last item of the sequence.</param>
		/// <returns>The last match.</returns>
		/// <exception cref="InvalidOperationException">Nothing matched.</exception>
		public static async Task<T> LastAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.Last(predicate))
				: Task.Run(() => sequence.Last())).ConfigureAwait(false);
		}

		/// <summary>Returns the last matching item, or the type's default.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the last item of the sequence.</param>
		/// <returns>The last match, or the type's default when nothing matched.</returns>
		public static async Task<T> LastOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.LastOrDefault(predicate))
				: Task.Run(() => sequence.LastOrDefault())).ConfigureAwait(false);
		}

		/// <summary>Determines whether any item matches.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> just checks that the sequence is not empty.</param>
		/// <returns><c>true</c> if at least one item matched.</returns>
		public static async Task<Boolean> AnyAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null)
		{
			return await (predicate != null
				? Task.Run(() => sequence.Any(predicate))
				: Task.Run(() => sequence.Any())).ConfigureAwait(false);
		}

		/// <summary>Determines whether every item matches.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter every item must satisfy.</param>
		/// <returns><c>true</c> if all items matched, including when the sequence is empty.</returns>
		public static async Task<Boolean> AllAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate)
		{
			return await Task.Run(() => sequence.All(predicate)).ConfigureAwait(false);
		}

		/// <summary>Materializes the sequence into an array.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to materialize.</param>
		/// <returns>An array holding the items.</returns>
		public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<T> sequence)
		{
			return await Task.Run(() => sequence.ToArray()).ConfigureAwait(false);
		}

		/// <summary>Materializes the sequence into a list.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to materialize.</param>
		/// <returns>A list holding the items.</returns>
		public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> sequence)
		{
			return await Task.Run(() => sequence.ToList()).ConfigureAwait(false);
		}
	}
}
