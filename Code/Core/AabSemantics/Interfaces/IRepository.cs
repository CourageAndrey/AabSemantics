using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// Storage for identifiable items, keyed by <see cref="IIdentifiable.ID"/>. The interface is
	/// asynchronous so that a semantic network can be backed by a database as readily as by memory;
	/// see <see cref="RepositoryExtensions"/> for synchronous wrappers.
	/// <para>
	/// Every member takes a cancellation token, as a repository which talks to a database has to be
	/// interruptible while it waits for one. An implementation backed by memory has nothing to wait
	/// for and only observes the token before it starts.
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type of the stored items.</typeparam>
	public interface IRepository<T> : IEnumerable<T>
		where T : IIdentifiable
	{
		/// <summary>
		/// Stores an item.
		/// </summary>
		/// <param name="item">Item to store; its identifier becomes its key.</param>
		/// <param name="cancellationToken">Cancels the call before the item is stored.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task AddAsync(T item, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels the call before the item is removed.</param>
		/// <returns><c>true</c> if the item was present and has been removed.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<Boolean> RemoveAsync(T item, CancellationToken cancellationToken = default);

		/// <summary>
		/// Removes every item.
		/// </summary>
		/// <param name="cancellationToken">Cancels the call before anything is removed.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task ClearAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Counts the stored items.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>Number of items currently stored.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<Int32> GetCountAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Looks an item up by key.
		/// </summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>The matching item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<T> GetItemAsync(String key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Lists the identifiers of every stored item.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>All keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<ICollection<String>> GetKeysAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Determines whether an item with the given key is stored.
		/// </summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<Boolean> ContainsAsync(String key, CancellationToken cancellationToken = default);

		/// <summary>
		/// Looks an item up without throwing when it is absent.
		/// </summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>
		/// A pair whose key reports success and whose value holds the item, or the type's
		/// default when nothing matched.
		/// </returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<KeyValuePair<Boolean, T>> TryGetValueAsync(String key, CancellationToken cancellationToken = default);
	}

	/// <summary>
	/// Blocking wrappers over <see cref="IRepository{T}"/> for callers that cannot await.
	/// Each one short-circuits to the synchronous path when the repository is the in-memory
	/// <see cref="Repository{T}"/>, and otherwise blocks on the asynchronous call. The token is
	/// observed up front either way, and on the asynchronous path it also travels into the
	/// repository, so another thread cancelling it can still cut a database wait short.
	/// </summary>
	public static class RepositoryExtensions
	{
		/// <summary>
		/// Stores an item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to modify.</param>
		/// <param name="item">Item to store; its identifier becomes its key.</param>
		/// <param name="cancellationToken">Cancels the call before the item is stored.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static void Add<T>(this IRepository<T> collection, T item, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				inMemory.Add(item);
			}
			else
			{
				TaskHelper.AwaitDetached(() => collection.AddAsync(item, cancellationToken));
			}
		}

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to modify.</param>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels the call before the item is removed.</param>
		/// <returns><c>true</c> if the item was present and has been removed.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Boolean Remove<T>(this IRepository<T> collection, T item, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Remove(item)
				: TaskHelper.AwaitDetached(() => collection.RemoveAsync(item, cancellationToken));
		}

		/// <summary>
		/// Removes every item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to empty.</param>
		/// <param name="cancellationToken">Cancels the call before anything is removed.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static void Clear<T>(this IRepository<T> collection, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				inMemory.Clear();
			}
			else
			{
				TaskHelper.AwaitDetached(() => collection.ClearAsync(cancellationToken));
			}
		}

		/// <summary>
		/// Counts the stored items.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to inspect.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>Number of items currently stored.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Int32 GetCount<T>(this IRepository<T> collection, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Count
				: TaskHelper.AwaitDetached(() => collection.GetCountAsync(cancellationToken));
		}

		/// <summary>
		/// Looks an item up by key.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>The matching item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static T GetItem<T>(this IRepository<T> collection, String key, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory[key]
				: TaskHelper.AwaitDetached(() => collection.GetItemAsync(key, cancellationToken));
		}

		/// <summary>
		/// Lists the identifiers of every stored item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to inspect.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns>All keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static IEnumerable<String> GetKeys<T>(this IRepository<T> collection, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Keys
				: TaskHelper.AwaitDetached(() => collection.GetKeysAsync(cancellationToken));
		}

		/// <summary>
		/// Determines whether an item with the given key is stored.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Boolean Contains<T>(this IRepository<T> collection, String key, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Contains(key)
				: TaskHelper.AwaitDetached(() => collection.ContainsAsync(key, cancellationToken));
		}

		/// <summary>
		/// Looks an item up without throwing when it is absent.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="value">Receives the matching item, or the type's default when nothing matched.</param>
		/// <param name="cancellationToken">Cancels waiting for the storage.</param>
		/// <returns><c>true</c> if an item was found.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Boolean TryGetValue<T>(this IRepository<T> collection, String key, out T value, CancellationToken cancellationToken = default)
			where T : IIdentifiable
		{
			cancellationToken.ThrowIfCancellationRequested();

			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				return inMemory.TryGetValue(key, out value);
			}
			else
			{
				var result = TaskHelper.AwaitDetached(() => collection.TryGetValueAsync(key, cancellationToken));
				if (result.Key)
				{
					value = result.Value;
					return true;
				}
				else
				{
					value = default;
					return false;
				}
			}
		}
	}
}
