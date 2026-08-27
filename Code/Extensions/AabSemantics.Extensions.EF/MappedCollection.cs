using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	/// <summary>Chooses which mapping an item should be written through.</summary>
	/// <typeparam name="ItemT">Item type.</typeparam>
	/// <param name="mappings">Available mappings.</param>
	/// <param name="forItem">Item about to be stored.</param>
	/// <returns>The mapping to use.</returns>
	internal delegate IMapping<ItemT> MappingSelectorDelegate<ItemT>(ICollection<IMapping<ItemT>> mappings, ItemT forItem)
		where ItemT : IIdentifiable;

	/// <summary>
	/// Repository combining several table mappings into one collection. Reads span every
	/// mapping; writes go to the one picked by <see cref="MappingSelector"/> and are staged
	/// until the owning network saves them.
	/// </summary>
	/// <typeparam name="ItemT">Item type.</typeparam>
	internal class MappedCollection<ItemT> : IRepository<ItemT>
		where ItemT : class, IIdentifiable
	{
		#region Properties

		private readonly List<IMapping<ItemT>> _mappings = new List<IMapping<ItemT>>();
		private MappingSelectorDelegate<ItemT> _mappingSelector;

		/// <summary>
		/// Chooses the mapping new items are written through. Assigning <c>null</c> restores
		/// the default, which always picks the first mapping.
		/// </summary>
		public MappingSelectorDelegate<ItemT> MappingSelector
		{
			get { return _mappingSelector; }
			set { _mappingSelector = value ?? ((mappings, item) => mappings.First()); }
		}

		#endregion

		/// <summary>Creates an empty collection with no mappings yet.</summary>
		/// <param name="mappingSelector">Chooses the mapping writes go to; <c>null</c> uses the first mapping.</param>
		public MappedCollection(MappingSelectorDelegate<ItemT> mappingSelector = null)
		{
			MappingSelector = mappingSelector;
		}

		/// <summary>Adds a table mapping to the collection.</summary>
		/// <typeparam name="EntityT">Entity type stored in the table.</typeparam>
		/// <param name="dbContext">Context owning the table; changes are saved through it.</param>
		/// <param name="dbSet">Table to map.</param>
		/// <param name="map">Converts an entity into an item.</param>
		/// <param name="mapBack">Converts an item into an entity.</param>
		/// <param name="getKey">Returns an entity's identifier.</param>
		/// <param name="mappingSelector">Unused; the collection-wide <see cref="MappingSelector"/> applies.</param>
		public void Map<EntityT>(
			DbContext dbContext,
			DbSet<EntityT> dbSet,
			Func<EntityT, ItemT> map,
			Func<ItemT, EntityT> mapBack,
			Func<EntityT, string> getKey,
			MappingSelectorDelegate<ItemT> mappingSelector = null)
			where EntityT : class
		{
			_mappings.Add(new Mapping<ItemT, EntityT>(dbContext, dbSet, map, mapBack, getKey));
		}

		#region Implementation of IRepository

		/// <summary>Enumerates the items of every mapping in registration order.</summary>
		/// <returns>An enumerator over all mapped items.</returns>
		public IEnumerator<ItemT> GetEnumerator()
		{
			foreach (var mapping in _mappings)
			{
				foreach (var item in TaskHelper.AwaitDetached(() => mapping.GetAllItemsAsync()))
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Stages an item through the mapping chosen by <see cref="MappingSelector"/>.</summary>
		/// <param name="item">Item to store.</param>
		public async Task AddAsync(ItemT item)
		{
			await _mappingSelector(_mappings, item).AddAsync(item).ConfigureAwait(false);
		}

		/// <summary>Stages an item's deletion, trying each mapping until one reports success.</summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> when some mapping found it.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
		public async Task<bool> RemoveAsync(ItemT item)
		{
			item.EnsureNotNull(nameof(item));

			foreach (var mapping in _mappings)
			{
				if (await mapping.RemoveAsync(item).ConfigureAwait(false))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>Stages the emptying of every mapped table.</summary>
		public async Task ClearAsync()
		{
			foreach (var mapping in _mappings)
			{
				await mapping.ClearAsync().ConfigureAwait(false);
			}
		}

		/// <summary>Counts the items across every mapping.</summary>
		/// <returns>Total number of items.</returns>
		public async Task<int> GetCountAsync()
		{
			int count = 0;
			foreach (var mapping in _mappings)
			{
				count += await mapping.GetCountAsync().ConfigureAwait(false);
			}

			return count;
		}

		/// <summary>Looks an item up by key.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>The matching item, or <c>null</c> when nothing matched.</returns>
		public async Task<ItemT> GetItemAsync(string key)
		{
			return (await TryGetValueAsync(key).ConfigureAwait(false)).Value;
		}

		/// <summary>Lists the identifiers of every item across all mappings.</summary>
		/// <returns>All keys currently in use.</returns>
		public async Task<ICollection<string>> GetKeysAsync()
		{
			var keys = new List<string>();
			foreach (var mapping in _mappings)
			{
				keys.AddRange(await mapping.GetKeysAsync().ConfigureAwait(false));
			}

			return keys;
		}

		/// <summary>Determines whether an item with the given key exists in any mapping.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		public async Task<bool> ContainsAsync(string key)
		{
			return (await TryGetValueAsync(key).ConfigureAwait(false)).Key;
		}

		/// <summary>Looks an item up across every mapping, returning the first match.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		public async Task<KeyValuePair<bool, ItemT>> TryGetValueAsync(string key)
		{
			foreach (var mapping in _mappings)
			{
				var result = await mapping.TryGetItemAsync(key).ConfigureAwait(false);
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
