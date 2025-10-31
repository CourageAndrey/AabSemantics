using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	internal delegate IMapping<ItemT> MappingSelectorDelegate<ItemT>(ICollection<IMapping<ItemT>> mappings, ItemT forItem)
		where ItemT : IIdentifiable;

	internal class MappedCollection<ItemT> : IRepository<ItemT>
		where ItemT : class, IIdentifiable
	{
		#region Properties

		private readonly List<IMapping<ItemT>> _mappings = new List<IMapping<ItemT>>();
		private MappingSelectorDelegate<ItemT> _mappingSelector;

		public MappingSelectorDelegate<ItemT> MappingSelector
		{
			get { return _mappingSelector; }
			set { _mappingSelector = value ?? ((mappings, item) => mappings.First()); }
		}

		#endregion

		public MappedCollection(MappingSelectorDelegate<ItemT> mappingSelector = null)
		{
			MappingSelector = mappingSelector;
		}

		public void Map<EntityT>(
			DbSet<EntityT> dbSet,
			Func<EntityT, ItemT> map,
			Func<ItemT, EntityT> mapBack,
			Func<EntityT, string> getKey,
			MappingSelectorDelegate<ItemT> mappingSelector = null)
			where EntityT : class
		{
			_mappings.Add(new Mapping<ItemT, EntityT>(dbSet, map, mapBack, getKey));
		}

		#region Implementation of IRepository

		public IEnumerator<ItemT> GetEnumerator()
		{
			foreach (var mapping in _mappings)
			{
				foreach (var item in mapping.GetAllItemsAsync().Await())
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public async Task AddAsync(ItemT item)
		{
			await _mappingSelector(_mappings, item).AddAsync(item);
		}

		public async Task<bool> RemoveAsync(ItemT item)
		{
			item.EnsureNotNull(nameof(item));

			foreach (var mapping in _mappings)
			{
				if (await mapping.RemoveAsync(item))
				{
					return true;
				}
			}

			return false;
		}

		public async Task ClearAsync()
		{
			foreach (var mapping in _mappings)
			{
				await mapping.ClearAsync();
			}
		}

		public async Task<int> GetCountAsync()
		{
			return await Task.FromResult(_mappings.Sum(mapping => mapping.GetCountAsync().Await()));
		}

		public async Task<ItemT> GetItemAsync(string key)
		{
			return (await TryGetValueAsync(key)).Value;
		}

		public async Task<ICollection<string>> GetKeysAsync()
		{
			return await Task.FromResult(_mappings.Aggregate(
					new List<string>(),
					(list, items) =>
					{
						list.AddRange(items.GetKeysAsync().Await());
						return list;
					},
					list => list));
		}

		public async Task<bool> ContainsAsync(string key)
		{
			return (await TryGetValueAsync(key)).Key;
		}

		public async Task<KeyValuePair<bool, ItemT>> TryGetValueAsync(string key)
		{
			foreach (var mapping in _mappings)
			{
				var result = await mapping.TryGetItemAsync(key);
				if (result.Key)
				{
					return result;
				}
			}

			return new KeyValuePair<bool, ItemT>(false, null);
		}

		#endregion
	}
}
