using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AabSemantics.Utils
{
	/// <summary>
	/// Asynchronous facade over the LINQ operators used during inference. The sequences involved are
	/// in-memory ones, so each operator runs on the calling thread and returns an already completed
	/// task: there is nothing to await, and offloading to the thread pool would only add a hop.
	/// The <see cref="CancellationToken"/> is observed between items, which lets a long traversal
	/// stop early.
	/// </summary>
	public static class AsyncSequences
	{
		/// <summary>Returns the first matching item.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the first item of the sequence.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>The first match.</returns>
		/// <exception cref="InvalidOperationException">Nothing matched.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<T> FirstAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null, CancellationToken cancellationToken = default)
		{
			return Execute(
				() => predicate != null
					? sequence.Observing(cancellationToken).First(predicate)
					: sequence.Observing(cancellationToken).First(),
				cancellationToken);
		}

		/// <summary>Returns the first matching item, or the type's default.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the first item of the sequence.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>The first match, or the type's default when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null, CancellationToken cancellationToken = default)
		{
			return Execute(
				() => predicate != null
					? sequence.Observing(cancellationToken).FirstOrDefault(predicate)
					: sequence.Observing(cancellationToken).FirstOrDefault(),
				cancellationToken);
		}

		/// <summary>Returns the last matching item.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the last item of the sequence.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>The last match.</returns>
		/// <exception cref="InvalidOperationException">Nothing matched.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<T> LastAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null, CancellationToken cancellationToken = default)
		{
			return Execute(
				() => predicate != null
					? sequence.Observing(cancellationToken).Last(predicate)
					: sequence.Observing(cancellationToken).Last(),
				cancellationToken);
		}

		/// <summary>Returns the last matching item, or the type's default.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> takes the last item of the sequence.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>The last match, or the type's default when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<T> LastOrDefaultAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null, CancellationToken cancellationToken = default)
		{
			return Execute(
				() => predicate != null
					? sequence.Observing(cancellationToken).LastOrDefault(predicate)
					: sequence.Observing(cancellationToken).LastOrDefault(),
				cancellationToken);
		}

		/// <summary>Determines whether any item matches.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter; <c>null</c> just checks that the sequence is not empty.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns><c>true</c> if at least one item matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<Boolean> AnyAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate = null, CancellationToken cancellationToken = default)
		{
			return Execute(
				() => predicate != null
					? sequence.Observing(cancellationToken).Any(predicate)
					: sequence.Observing(cancellationToken).Any(),
				cancellationToken);
		}

		/// <summary>Determines whether every item matches.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="predicate">Filter every item must satisfy.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns><c>true</c> if all items matched, including when the sequence is empty.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<Boolean> AllAsync<T>(this IEnumerable<T> sequence, Func<T, Boolean> predicate, CancellationToken cancellationToken = default)
		{
			return Execute(() => sequence.Observing(cancellationToken).All(predicate), cancellationToken);
		}

		/// <summary>Materializes the sequence into an array.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to materialize.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>An array holding the items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<T[]> ToArrayAsync<T>(this IEnumerable<T> sequence, CancellationToken cancellationToken = default)
		{
			return Execute(() => sequence.Observing(cancellationToken).ToArray(), cancellationToken);
		}

		/// <summary>Materializes the sequence into a list.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to materialize.</param>
		/// <param name="cancellationToken">Cancels the enumeration.</param>
		/// <returns>A list holding the items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> sequence, CancellationToken cancellationToken = default)
		{
			return Execute(() => sequence.Observing(cancellationToken).ToList(), cancellationToken);
		}

		private static Task<T> Execute<T>(Func<T> operation, CancellationToken cancellationToken)
		{
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				return Task.FromResult(operation());
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<T>(cancellationToken);
			}
			catch (Exception error)
			{
				return Task.FromException<T>(error);
			}
		}

		private static IEnumerable<T> Observing<T>(this IEnumerable<T> sequence, CancellationToken cancellationToken)
		{
			return cancellationToken.CanBeCanceled
				? ObservingIterator(sequence, cancellationToken)
				: sequence;
		}

		private static IEnumerable<T> ObservingIterator<T>(IEnumerable<T> sequence, CancellationToken cancellationToken)
		{
			foreach (var item in sequence)
			{
				cancellationToken.ThrowIfCancellationRequested();
				yield return item;
			}
		}
	}
}
