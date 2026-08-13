using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	/// <summary>Type-agnostic part of a table mapping.</summary>
	internal interface IMapping
	{
		/// <summary>Counts the rows in the mapped table.</summary>
		/// <returns>Number of rows.</returns>
		Task<int> GetCountAsync();

		/// <summary>Lists the identifiers of every row.</summary>
		/// <returns>All keys in the mapped table.</returns>
		Task<IEnumerable<string>> GetKeysAsync();

		/// <summary>Deletes every row from the mapped table.</summary>
		Task ClearAsync();
	}

	/// <summary>Mapping between a database table and semantic network items.</summary>
	/// <typeparam name="ItemT">Item type the rows represent.</typeparam>
	internal interface IMapping<ItemT> : IMapping
		where ItemT : IIdentifiable
	{
		/// <summary>Reads every row as an item.</summary>
		/// <returns>All mapped items.</returns>
		Task<IEnumerable<ItemT>> GetAllItemsAsync();

		/// <summary>Reads a single item by key.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		Task<KeyValuePair<bool, ItemT>> TryGetItemAsync(string key);

		/// <summary>Inserts an item as a new row.</summary>
		/// <param name="item">Item to store.</param>
		/// <returns><c>true</c> when the row was added.</returns>
		Task<bool> AddAsync(ItemT item);

		/// <summary>Deletes the row matching an item's identifier.</summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> when a matching row was found and deleted.</returns>
		Task<bool> RemoveAsync(ItemT item);
	}

	/// <summary>Mapping that exposes the table it is bound to.</summary>
	/// <typeparam name="ItemT">Item type the rows represent.</typeparam>
	/// <typeparam name="EntityT">Entity type stored in the table.</typeparam>
	internal interface IMapping<ItemT, EntityT> : IMapping<ItemT>
		where ItemT : IIdentifiable
		where EntityT : class
	{
		/// <summary>The mapped table.</summary>
		DbSet<EntityT> DbSet
		{ get; }
	}

	/// <summary>
	/// Default table mapping. Lookups enumerate the whole table client-side rather than
	/// translating the key comparison into SQL, so this does not scale to large tables.
	/// </summary>
	/// <typeparam name="ItemT">Item type the rows represent.</typeparam>
	/// <typeparam name="EntityT">Entity type stored in the table.</typeparam>
	internal class Mapping<ItemT, EntityT> : IMapping<ItemT, EntityT>
		where ItemT : IIdentifiable
		where EntityT : class
	{
#warning Not effective way of implementation!

		#region Properties

		/// <summary>The mapped table.</summary>
		public DbSet<EntityT> DbSet
		{ get; }

		private readonly Func<EntityT, ItemT> _map;
		private readonly Func<ItemT, EntityT> _mapBack;
		private readonly Func<EntityT, string> _getKey;

		#endregion

		/// <summary>Creates a mapping between a table and an item type.</summary>
		/// <param name="dbSet">Table to map.</param>
		/// <param name="map">Converts an entity into an item.</param>
		/// <param name="mapBack">Converts an item into an entity.</param>
		/// <param name="getKey">Returns an entity's identifier.</param>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
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

		/// <summary>Counts the rows in the mapped table.</summary>
		/// <returns>Number of rows.</returns>
		public async Task<int> GetCountAsync()
		{
			return await Task.FromResult(DbSet.Count());
		}

		/// <summary>Lists the identifiers of every row.</summary>
		/// <returns>All keys in the mapped table.</returns>
		public Task<IEnumerable<string>> GetKeysAsync()
		{
			return Task.Run(() => DbSet.AsEnumerable().Select(item => _getKey(item)));
		}

		/// <summary>Reads every row as an item.</summary>
		/// <returns>All mapped items.</returns>
		public Task<IEnumerable<ItemT>> GetAllItemsAsync()
		{
			return Task.Run(() => DbSet.AsEnumerable().Select(item => _map(item)));
		}

		/// <summary>Reads a single item by key.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
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

		/// <summary>Inserts an item as a new row.</summary>
		/// <param name="item">Item to store.</param>
		/// <returns>Always <c>true</c>.</returns>
		public Task<bool> AddAsync(ItemT item)
		{
			DbSet.Add(_mapBack(item));
			return Task.FromResult(true);
		}

		/// <summary>Deletes the row matching an item's identifier.</summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> when a matching row was found and deleted.</returns>
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

		/// <summary>Deletes every row from the mapped table.</summary>
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
