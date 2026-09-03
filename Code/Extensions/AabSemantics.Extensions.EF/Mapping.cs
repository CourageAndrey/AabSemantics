using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	/// <summary>Type-agnostic part of a table mapping.</summary>
	internal interface IMapping
	{
		/// <summary>Counts the rows in the mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>Number of rows.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<int> GetCountAsync(CancellationToken cancellationToken = default);

		/// <summary>Lists the identifiers of every row.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All keys in the mapped table.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<IEnumerable<string>> GetKeysAsync(CancellationToken cancellationToken = default);

		/// <summary>Stages the deletion of every row of the mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task ClearAsync(CancellationToken cancellationToken = default);
	}

	/// <summary>Mapping between a database table and semantic network items.</summary>
	/// <typeparam name="ItemT">Item type the rows represent.</typeparam>
	internal interface IMapping<ItemT> : IMapping
		where ItemT : IIdentifiable
	{
		/// <summary>Reads every row as an item.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<IEnumerable<ItemT>> GetAllItemsAsync(CancellationToken cancellationToken = default);

		/// <summary>Reads a single item by key.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<KeyValuePair<bool, ItemT>> TryGetItemAsync(string key, CancellationToken cancellationToken = default);

		/// <summary>Stages an item as a new row.</summary>
		/// <param name="item">Item to store.</param>
		/// <param name="cancellationToken">Cancels the call before the row is staged.</param>
		/// <returns><c>true</c> when the row was staged.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<bool> AddAsync(ItemT item, CancellationToken cancellationToken = default);

		/// <summary>Stages the deletion of the row matching an item's identifier.</summary>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <returns><c>true</c> when a matching row was found and staged for deletion.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		Task<bool> RemoveAsync(ItemT item, CancellationToken cancellationToken = default);
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
	/// Default table mapping. Every database access goes through the asynchronous Entity Framework
	/// API, so the calling thread is released for the duration of the round trip and the wait can be
	/// cut short by the cancellation token every method takes. Writes are only
	/// staged in the change tracker: they reach the database when the owning
	/// <see cref="DbSemanticNetwork{ContextT}"/> saves them, and reads report them meanwhile.
	/// Lookups still enumerate the whole table client-side rather than translating the key
	/// comparison into SQL, because the key is produced by a delegate that LINQ to Entities cannot
	/// translate, so this does not scale to large tables.
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

		private readonly DbContext _dbContext;
		private readonly Func<EntityT, ItemT> _map;
		private readonly Func<ItemT, EntityT> _mapBack;
		private readonly Func<EntityT, string> _getKey;

		#endregion

		/// <summary>Creates a mapping between a table and an item type.</summary>
		/// <param name="dbContext">Context owning the table; changes are saved through it.</param>
		/// <param name="dbSet">Table to map.</param>
		/// <param name="map">Converts an entity into an item.</param>
		/// <param name="mapBack">Converts an item into an entity.</param>
		/// <param name="getKey">Returns an entity's identifier.</param>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
		public Mapping(
			DbContext dbContext,
			DbSet<EntityT> dbSet,
			Func<EntityT, ItemT> map,
			Func<ItemT, EntityT> mapBack,
			Func<EntityT, string> getKey)
		{
			_dbContext = dbContext.EnsureNotNull(nameof(dbContext));
			DbSet = dbSet.EnsureNotNull(nameof(dbSet));
			_map = map.EnsureNotNull(nameof(map));
			_mapBack = mapBack.EnsureNotNull(nameof(mapBack));
			_getKey = getKey.EnsureNotNull(nameof(getKey));
		}

		/// <summary>Counts the rows in the mapped table, pending changes included.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>Number of rows.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
		{
			// as long as nothing is staged, the count can be left to the database
			return _dbContext.ChangeTracker.HasChanges()
				? (await ReadEntitiesAsync(cancellationToken).ConfigureAwait(false)).Count
				: await DbSet.CountAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <summary>Lists the identifiers of every row, pending changes included.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All keys in the mapped table.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<IEnumerable<string>> GetKeysAsync(CancellationToken cancellationToken = default)
		{
			var entities = await ReadEntitiesAsync(cancellationToken).ConfigureAwait(false);
			return entities.Select(entity => _getKey(entity)).ToList();
		}

		/// <summary>Reads every row as an item, pending changes included.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<IEnumerable<ItemT>> GetAllItemsAsync(CancellationToken cancellationToken = default)
		{
			var entities = await ReadEntitiesAsync(cancellationToken).ConfigureAwait(false);
			return entities.Select(entity => _map(entity)).ToList();
		}

		/// <summary>Reads a single item by key, pending changes included.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<KeyValuePair<bool, ItemT>> TryGetItemAsync(string key, CancellationToken cancellationToken = default)
		{
			var entity = await FindEntityAsync(key, cancellationToken).ConfigureAwait(false);
			return entity != null
				? new KeyValuePair<bool, ItemT>(true, _map(entity))
				: new KeyValuePair<bool, ItemT>(false, default);
		}

		/// <summary>Stages an item as a new row. Nothing is written until the changes are saved.</summary>
		/// <param name="item">Item to store.</param>
		/// <param name="cancellationToken">Cancels the call before the row is staged.</param>
		/// <returns>Always <c>true</c>.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<bool> AddAsync(ItemT item, CancellationToken cancellationToken = default)
		{
			// staging touches the change tracker only, so there is no round trip to await here and
			// the token can only be observed before the item is staged
			return TaskHelper.FromSynchronous(
				() =>
				{
					DbSet.Add(_mapBack(item));
					return true;
				},
				cancellationToken);
		}

		/// <summary>Stages the deletion of the row matching an item's identifier.</summary>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <returns><c>true</c> when a matching row was found and staged for deletion.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<bool> RemoveAsync(ItemT item, CancellationToken cancellationToken = default)
		{
			var entity = await FindEntityAsync(item.ID, cancellationToken).ConfigureAwait(false);
			if (entity == null)
			{
				return false;
			}

			DbSet.Remove(entity);
			return true;
		}

		/// <summary>Stages the deletion of every row of the mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task ClearAsync(CancellationToken cancellationToken = default)
		{
			DbSet.RemoveRange(await ReadEntitiesAsync(cancellationToken).ConfigureAwait(false));
		}

		/// <summary>
		/// Finds the entity a key belongs to. The whole table has to be materialized first, as the
		/// key is calculated in memory.
		/// </summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>The matching entity, or <c>null</c> when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		private async Task<EntityT> FindEntityAsync(string key, CancellationToken cancellationToken)
		{
			var entities = await ReadEntitiesAsync(cancellationToken).ConfigureAwait(false);
			return entities.FirstOrDefault(entity => _getKey(entity) == key);
		}

		/// <summary>
		/// Materializes the table. Only the query itself is cancellable: once the rows have arrived,
		/// merging the staged changes into them is in-memory work that runs to completion.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>The stored rows, pending changes applied.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		private async Task<List<EntityT>> ReadEntitiesAsync(CancellationToken cancellationToken)
		{
			var stored = await DbSet.ToListAsync(cancellationToken).ConfigureAwait(false);

			var entities = stored.Where(entity => _dbContext.Entry(entity).State != EntityState.Deleted).ToList();
			entities.AddRange(DbSet.Local.Where(entity => _dbContext.Entry(entity).State == EntityState.Added));
			return entities;
		}
	}
}
