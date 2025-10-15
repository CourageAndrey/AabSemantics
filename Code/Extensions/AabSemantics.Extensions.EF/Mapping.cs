using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	internal interface IMapping
	{
		Task<int> GetCountAsync();

		Task<IEnumerable<string>> GetKeysAsync();

		Task ClearAsync();
	}

	internal interface IMapping<ItemT> : IMapping
		where ItemT : IIdentifiable
	{
		Task<IEnumerable<ItemT>> GetAllItemsAsync();

		Task<KeyValuePair<bool, ItemT>> TryGetItemAsync(string key);

		Task<bool> AddAsync(ItemT item);

		Task<bool> RemoveAsync(ItemT item);
	}

	internal interface IMapping<ItemT, EntityT> : IMapping<ItemT>
		where ItemT : IIdentifiable
		where EntityT : class
	{
		DbSet<EntityT> DbSet
		{ get; }
	}

	internal class Mapping<ItemT, EntityT> : IMapping<ItemT, EntityT>
		where ItemT : IIdentifiable
		where EntityT : class
	{
#warning Not effective way of implementation!

		#region Properties

		public DbSet<EntityT> DbSet
		{ get; }

		private readonly Func<EntityT, ItemT> _map;
		private readonly Func<ItemT, EntityT> _mapBack;
		private readonly Func<EntityT, string> _getKey;

		#endregion

		public Mapping(
			DbSet<EntityT> dbSet,
			Func<EntityT, ItemT> map,
			Func<ItemT, EntityT> mapBack,
			Func<EntityT, string> getKey)
		{
			DbSet = dbSet.EnsureNotNull(nameof(dbSet));
			_map = map.EnsureNotNull(nameof(map));
			_mapBack = mapBack.EnsureNotNull(nameof(mapBack));
			_getKey = getKey.EnsureNotNull(nameof(getKey));
		}

		public async Task<int> GetCountAsync()
		{
			return await Task.FromResult(DbSet.Count());
		}

		public Task<IEnumerable<string>> GetKeysAsync()
		{
			return Task.Run(() => DbSet.AsEnumerable().Select(item => _getKey(item)));
		}

		public Task<IEnumerable<ItemT>> GetAllItemsAsync()
		{
			return Task.Run(() => DbSet.AsEnumerable().Select(item => _map(item)));
		}

		public Task<KeyValuePair<bool, ItemT>> TryGetItemAsync(string key)
		{
			return Task.Run(() =>
			{
				var search = DbSet.AsEnumerable().Where(i => _getKey(i) == key);
				if (search.Any())
				{
					return new KeyValuePair<bool, ItemT>(true, _map(search.First()));
				}
				else
				{
					return new KeyValuePair<bool, ItemT>(false, default);
				}
			});
		}

		public Task<bool> AddAsync(ItemT item)
		{
			DbSet.Add(_mapBack(item));
			return Task.FromResult(true);
		}

		public Task<bool> RemoveAsync(ItemT item)
		{
			return Task.Run(() =>
			{
				foreach (var entity in DbSet)
				{
					if (_getKey(entity) == item.ID)
					{
						DbSet.Remove(entity);
						return true;
					}
				}

				return false;
			});
		}

		public Task ClearAsync()
		{
			return Task.Run(() =>
			{
				foreach (var entity in DbSet.ToList())
				{
					DbSet.Remove(entity);
				}
			});
		}
	}
}
