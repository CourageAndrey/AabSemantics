using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	internal delegate IMapping<ItemT> MappingSelectorDelegate<ItemT>(ICollection<IMapping<ItemT>> mappings, ItemT forItem)
		where ItemT : IIdentifiable;

	internal class MappedCollection<ItemT> : IKeyedCollection<ItemT>
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

		#region Implementation of IKeyedCollection

		public IEnumerator<ItemT> GetEnumerator()
		{
			foreach (var mapping in _mappings)
			{
				foreach (var item in mapping.GetAllItems())
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(ItemT item)
		{
			_mappingSelector(_mappings, item).Add(item);
		}

		public void Clear()
		{
			foreach (var mapping in _mappings)
			{
				mapping.Clear();
			}
		}

		public bool Contains(ItemT item)
		{
			item.EnsureNotNull(nameof(item));

			ItemT temp;
			return _mappings.Any(mapping => mapping.TryGetItem(item.ID, out temp));
		}

		public void CopyTo(ItemT[] array, int arrayIndex)
		{
			foreach (var mapping in _mappings)
			{
				foreach (var item in mapping.GetAllItems())
				{
					array[arrayIndex++] = item;
				}
			}
		}

		public bool Remove(ItemT item)
		{
			item.EnsureNotNull(nameof(item));

			foreach (var mapping in _mappings)
			{
				if (mapping.Remove(item))
				{
					return true;
				}
			}

			return false;
		}

		public int Count
		{ get { return _mappings.Sum(mapping => mapping.Count); } }

		public bool IsReadOnly
		{ get { return false; } }

		public ItemT this[string key]
		{
			get
			{
				ItemT result;
				if (TryGetValue(key, out result))
				{
					return result;
				}
				else
				{
					throw new KeyNotFoundException();
				}
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return _mappings.Aggregate(
					new List<string>(),
					(list, items) =>
					{
						list.AddRange(items.GetKeys());
						return list;
					},
					list => list);
			}
		}

		public bool TryGetValue(string key, out ItemT value)
		{
			foreach (var mapping in _mappings)
			{
				if (mapping.TryGetItem(key, out value))
				{
					return true;
				}
			}

			value = default;
			return false;
		}

		#endregion
	}
}
