using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// Storage for identifiable items, keyed by <see cref="IIdentifiable.ID"/>. The interface is
	/// asynchronous so that a semantic network can be backed by a database as readily as by memory;
	/// see <see cref="RepositoryExtensions"/> for synchronous wrappers.
	/// </summary>
	/// <typeparam name="T">Type of the stored items.</typeparam>
	public interface IRepository<T> : IEnumerable<T>
		where T : IIdentifiable
	{
		/// <summary>
		/// Stores an item.
		/// </summary>
		/// <param name="item">Item to store; its identifier becomes its key.</param>
		Task AddAsync(T item);

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> if the item was present and has been removed.</returns>
		Task<Boolean> RemoveAsync(T item);

		/// <summary>
		/// Removes every item.
		/// </summary>
		Task ClearAsync();

		/// <summary>
		/// Counts the stored items.
		/// </summary>
		/// <returns>Number of items currently stored.</returns>
		Task<Int32> GetCountAsync();

		/// <summary>
		/// Looks an item up by key.
		/// </summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>The matching item.</returns>
		Task<T> GetItemAsync(String key);

		/// <summary>
		/// Lists the identifiers of every stored item.
		/// </summary>
		/// <returns>All keys currently in use.</returns>
		Task<ICollection<String>> GetKeysAsync();

		/// <summary>
		/// Determines whether an item with the given key is stored.
		/// </summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		Task<Boolean> ContainsAsync(String key);

		/// <summary>
		/// Looks an item up without throwing when it is absent.
		/// </summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>
		/// A pair whose key reports success and whose value holds the item, or the type's
		/// default when nothing matched.
		/// </returns>
		Task<KeyValuePair<Boolean, T>> TryGetValueAsync(String key);
	}

	/// <summary>
	/// Blocking wrappers over <see cref="IRepository{T}"/> for callers that cannot await.
	/// Each one short-circuits to the synchronous path when the repository is the in-memory
	/// <see cref="Repository{T}"/>, and otherwise blocks on the asynchronous call.
	/// </summary>
	public static class RepositoryExtensions
	{
		/// <summary>
		/// Stores an item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to modify.</param>
		/// <param name="item">Item to store; its identifier becomes its key.</param>
		public static void Add<T>(this IRepository<T> collection, T item)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				inMemory.Add(item);
			}
			else
			{
				TaskHelper.AwaitDetached(() => collection.AddAsync(item));
			}
		}

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to modify.</param>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> if the item was present and has been removed.</returns>
		public static Boolean Remove<T>(this IRepository<T> collection, T item)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Remove(item)
				: TaskHelper.AwaitDetached(() => collection.RemoveAsync(item));
		}

		/// <summary>
		/// Removes every item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to empty.</param>
		public static void Clear<T>(this IRepository<T> collection)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				inMemory.Clear();
			}
			else
			{
				TaskHelper.AwaitDetached(() => collection.ClearAsync());
			}
		}

		/// <summary>
		/// Counts the stored items.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to inspect.</param>
		/// <returns>Number of items currently stored.</returns>
		public static Int32 GetCount<T>(this IRepository<T> collection)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Count
				: TaskHelper.AwaitDetached(() => collection.GetCountAsync());
		}

		/// <summary>
		/// Looks an item up by key.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>The matching item.</returns>
		public static T GetItem<T>(this IRepository<T> collection, String key)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory[key]
				: TaskHelper.AwaitDetached(() => collection.GetItemAsync(key));
		}

		/// <summary>
		/// Lists the identifiers of every stored item.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to inspect.</param>
		/// <returns>All keys currently in use.</returns>
		public static IEnumerable<String> GetKeys<T>(this IRepository<T> collection)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Keys
				: TaskHelper.AwaitDetached(() => collection.GetKeysAsync());
		}

		/// <summary>
		/// Determines whether an item with the given key is stored.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier to look for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		public static Boolean Contains<T>(this IRepository<T> collection, String key)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Contains(key)
				: TaskHelper.AwaitDetached(() => collection.ContainsAsync(key));
		}

		/// <summary>
		/// Looks an item up without throwing when it is absent.
		/// </summary>
		/// <typeparam name="T">Type of the stored items.</typeparam>
		/// <param name="collection">Repository to search.</param>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="value">Receives the matching item, or the type's default when nothing matched.</param>
		/// <returns><c>true</c> if an item was found.</returns>
		public static Boolean TryGetValue<T>(this IRepository<T> collection, String key, out T value)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			if (inMemory != null)
			{
				return inMemory.TryGetValue(key, out value);
			}
			else
			{
				var result = TaskHelper.AwaitDetached(() => collection.TryGetValueAsync(key));
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
