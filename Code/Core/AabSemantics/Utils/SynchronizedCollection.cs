using System;
using System.Collections;
using System.Collections.Generic;

namespace AabSemantics.Utils
{
	public class SynchronizedCollection<T> : ICollection<T>
	{
		#region Properties

		private readonly ICollection<T> _items;
		private readonly Object _lock = new Object();

		#endregion

		#region Constructors

		public SynchronizedCollection()
			: this(new List<T>())
		{ }

		public SynchronizedCollection(ICollection<T> items)
		{
			_items = items.EnsureNotNull(nameof(items));
		}

		#endregion

		#region Implementation of ICollection

		public Int32 Count
		{
			get
			{
				lock (_lock)
				{
					return _items.Count;
				}
			}
		}

		public Boolean IsReadOnly
		{ get { return false; } }

		public void Add(T item)
		{
			lock (_lock)
			{
				_items.Add(item);
			}
		}

		public Boolean Remove(T item)
		{
			lock (_lock)
			{
				return _items.Remove(item);
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_items.Clear();
			}
		}

		public Boolean Contains(T item)
		{
			lock (_lock)
			{
				return _items.Contains(item);
			}
		}

		public void CopyTo(T[] array, Int32 arrayIndex)
		{
			lock (_lock)
			{
				_items.CopyTo(array, arrayIndex);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return CreateCopy().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		public List<T> CreateCopy()
		{
			lock (_lock)
			{
				return new List<T>(_items);
			}
		}
	}
}
