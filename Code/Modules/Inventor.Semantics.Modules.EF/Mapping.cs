using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.EF
{
	internal interface IMapping
	{
		int Count
		{ get; }

		IEnumerable<string> GetKeys();

		void Clear();
	}

	internal interface IMapping<ItemT> : IMapping
		where ItemT : IIdentifiable
	{
		IEnumerable<ItemT> GetAllItems();

		bool TryGetItem(string key, out ItemT item);

		bool Add(ItemT item);

		bool Remove(ItemT item);
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
		#region Properties

		public int Count
		{ get { return DbSet.Count(); } }

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

		public IEnumerable<string> GetKeys()
		{
			return DbSet.AsEnumerable().Select(item => _getKey(item));
		}

		public IEnumerable<ItemT> GetAllItems()
		{
			return DbSet.AsEnumerable().Select(item => _map(item));
		}

		public bool TryGetItem(string key, out ItemT item)
		{
#warning Not effective way of implementation!
			var search = DbSet.AsEnumerable().Where(i => _getKey(i) == key);
			if (search.Any())
			{
				item = _map(search.First());
				return true;
			}
			else
			{
				item = default;
				return false;
			}
		}

		public bool Add(ItemT item)
		{
			DbSet.Add(_mapBack(item));
			return true;
		}

		public bool Remove(ItemT item)
		{
#warning Not effective way of implementation!
			foreach (var entity in DbSet)
			{
				if (_getKey(entity) == item.ID)
				{
					DbSet.Remove(entity);
				}
				return true;
			}

			return false;
		}

		public void Clear()
		{
#warning Not effective way of implementation!
			foreach (var entity in DbSet.ToList())
			{
				DbSet.Remove(entity);
			}
		}
	}
}
