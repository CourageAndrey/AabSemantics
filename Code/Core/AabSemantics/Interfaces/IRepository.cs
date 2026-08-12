using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	public interface IRepository<T> : IEnumerable<T>
		where T : IIdentifiable
	{
		Task AddAsync(T item);

		Task<Boolean> RemoveAsync(T item);

		Task ClearAsync();

		Task<Int32> GetCountAsync();

		Task<T> GetItemAsync(String key);

		Task<ICollection<String>> GetKeysAsync();

		Task<Boolean> ContainsAsync(String key);

		Task<KeyValuePair<Boolean, T>> TryGetValueAsync(String key);
	}

	public static class RepositoryExtensions
	{
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

		public static Boolean Remove<T>(this IRepository<T> collection, T item)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Remove(item)
				: TaskHelper.AwaitDetached(() => collection.RemoveAsync(item));
		}

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

		public static Int32 GetCount<T>(this IRepository<T> collection)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Count
				: TaskHelper.AwaitDetached(() => collection.GetCountAsync());
		}

		public static T GetItem<T>(this IRepository<T> collection, String key)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory[key]
				: TaskHelper.AwaitDetached(() => collection.GetItemAsync(key));
		}

		public static IEnumerable<String> GetKeys<T>(this IRepository<T> collection)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Keys
				: TaskHelper.AwaitDetached(() => collection.GetKeysAsync());
		}

		public static Boolean Contains<T>(this IRepository<T> collection, String key)
			where T : IIdentifiable
		{
			var inMemory = collection as Repository<T>;
			return inMemory != null
				? inMemory.Contains(key)
				: TaskHelper.AwaitDetached(() => collection.ContainsAsync(key));
		}

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
