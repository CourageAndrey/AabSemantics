using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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
	/// <para>
	/// <see cref="IRepository{T}"/> knows nothing of cancellation, so every one of its members has a
	/// counterpart here that takes a token and is the one to call when the wait for the database has
	/// to be interruptible; the interface members delegate to it with
	/// <see cref="CancellationToken.None"/>.
	/// </para>
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
		public Task AddAsync(ItemT item)
		{
			return AddAsync(item, CancellationToken.None);
		}

		/// <summary>Stages an item's deletion, trying each mapping until one reports success.</summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> when some mapping found it.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
		public Task<bool> RemoveAsync(ItemT item)
		{
			return RemoveAsync(item, CancellationToken.None);
		}

		/// <summary>Stages the emptying of every mapped table.</summary>
		public Task ClearAsync()
		{
			return ClearAsync(CancellationToken.None);
		}

		/// <summary>Counts the items across every mapping.</summary>
		/// <returns>Total number of items.</returns>
		public Task<int> GetCountAsync()
		{
			return GetCountAsync(CancellationToken.None);
		}

		/// <summary>Looks an item up by key.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>The matching item, or <c>null</c> when nothing matched.</returns>
		public Task<ItemT> GetItemAsync(string key)
		{
			return GetItemAsync(key, CancellationToken.None);
		}

		/// <summary>Lists the identifiers of every item across all mappings.</summary>
		/// <returns>All keys currently in use.</returns>
		public Task<ICollection<string>> GetKeysAsync()
		{
			return GetKeysAsync(CancellationToken.None);
		}

		/// <summary>Determines whether an item with the given key exists in any mapping.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		public Task<bool> ContainsAsync(string key)
		{
			return ContainsAsync(key, CancellationToken.None);
		}

		/// <summary>Looks an item up across every mapping, returning the first match.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		public Task<KeyValuePair<bool, ItemT>> TryGetValueAsync(string key)
		{
			return TryGetValueAsync(key, CancellationToken.None);
		}

		#endregion

		#region Cancellable counterparts

		/// <summary>
		/// Stages an item through the mapping chosen by <see cref="MappingSelector"/>.
		/// Cancellable counterpart of <see cref="AddAsync(ItemT)"/>.
		/// </summary>
		/// <param name="item">Item to store.</param>
		/// <param name="cancellationToken">Cancels the call before the item is staged.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task AddAsync(ItemT item, CancellationToken cancellationToken)
		{
			await _mappingSelector(_mappings, item).AddAsync(item, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Stages an item's deletion, trying each mapping until one reports success. Cancellable
		/// counterpart of <see cref="RemoveAsync(ItemT)"/>; whatever earlier mappings have staged
		/// stays staged when the token is cancelled.
		/// </summary>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels the search through the mappings.</param>
		/// <returns><c>true</c> when some mapping found it.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<bool> RemoveAsync(ItemT item, CancellationToken cancellationToken)
		{
			item.EnsureNotNull(nameof(item));

			foreach (var mapping in _mappings)
			{
				if (await mapping.RemoveAsync(item, cancellationToken).ConfigureAwait(false))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Stages the emptying of every mapped table. Cancellable counterpart of
		/// <see cref="ClearAsync()"/>; the tables already visited stay staged for emptying when the
		/// token is cancelled.
		/// </summary>
		/// <param name="cancellationToken">Cancels the walk through the mappings.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task ClearAsync(CancellationToken cancellationToken)
		{
			foreach (var mapping in _mappings)
			{
				await mapping.ClearAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		/// <summary>Cancellable counterpart of <see cref="GetCountAsync()"/>.</summary>
		/// <param name="cancellationToken">Cancels the walk through the mappings.</param>
		/// <returns>Total number of items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<int> GetCountAsync(CancellationToken cancellationToken)
		{
			int count = 0;
			foreach (var mapping in _mappings)
			{
				count += await mapping.GetCountAsync(cancellationToken).ConfigureAwait(false);
			}

			return count;
		}

		/// <summary>Cancellable counterpart of <see cref="GetItemAsync(string)"/>.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels the search through the mappings.</param>
		/// <returns>The matching item, or <c>null</c> when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<ItemT> GetItemAsync(string key, CancellationToken cancellationToken)
		{
			return (await TryGetValueAsync(key, cancellationToken).ConfigureAwait(false)).Value;
		}

		/// <summary>Cancellable counterpart of <see cref="GetKeysAsync()"/>.</summary>
		/// <param name="cancellationToken">Cancels the walk through the mappings.</param>
		/// <returns>All keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<ICollection<string>> GetKeysAsync(CancellationToken cancellationToken)
		{
			var keys = new List<string>();
			foreach (var mapping in _mappings)
			{
				keys.AddRange(await mapping.GetKeysAsync(cancellationToken).ConfigureAwait(false));
			}

			return keys;
		}

		/// <summary>Cancellable counterpart of <see cref="ContainsAsync(string)"/>.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels the search through the mappings.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken)
		{
			return (await TryGetValueAsync(key, cancellationToken).ConfigureAwait(false)).Key;
		}

		/// <summary>Cancellable counterpart of <see cref="TryGetValueAsync(string)"/>.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels the search through the mappings.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<KeyValuePair<bool, ItemT>> TryGetValueAsync(string key, CancellationToken cancellationToken)
		{
			foreach (var mapping in _mappings)
			{
				var result = await mapping.TryGetItemAsync(key, cancellationToken).ConfigureAwait(false);
				if (result.Key)
				{
					return result;
				}
			}

			return new KeyValuePair<bool, ItemT>(false, null);
		}

		/// <summary>
		/// Reads the items of every mapping in registration order. Cancellable counterpart of
		/// <see cref="GetEnumerator"/>, which cannot take a token and reads one mapping at a time as
		/// it is enumerated; this one materializes them all.
		/// </summary>
		/// <param name="cancellationToken">Cancels the walk through the mappings.</param>
		/// <returns>All mapped items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<ICollection<ItemT>> GetAllItemsAsync(CancellationToken cancellationToken)
		{
			var items = new List<ItemT>();
			foreach (var mapping in _mappings)
			{
				items.AddRange(await mapping.GetAllItemsAsync(cancellationToken).ConfigureAwait(false));
			}

			return items;
		}

		#endregion
	}
}
