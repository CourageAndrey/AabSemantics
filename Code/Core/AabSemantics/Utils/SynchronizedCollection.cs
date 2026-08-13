using System;
using System.Collections;
using System.Collections.Generic;

namespace AabSemantics.Utils
{
	/// <summary>
	/// Collection guarding every operation with a lock. Enumeration iterates a snapshot rather
	/// than the live collection, so it is safe to modify the collection while enumerating —
	/// at the cost of copying it on each <see cref="GetEnumerator"/> call.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class SynchronizedCollection<T> : ICollection<T>
	{
		#region Properties

		private readonly ICollection<T> _items;
		private readonly Object _lock = new Object();

		#endregion

		#region Constructors

		/// <summary>Creates an empty synchronized collection backed by a list.</summary>
		public SynchronizedCollection()
			: this(new List<T>())
		{ }

		/// <summary>Wraps an existing collection. Bypassing this wrapper to touch it directly defeats the locking.</summary>
		/// <param name="items">Collection to guard.</param>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> is <c>null</c>.</exception>
		public SynchronizedCollection(ICollection<T> items)
		{
			_items = items.EnsureNotNull(nameof(items));
		}

		#endregion

		#region Implementation of ICollection

		/// <summary>Number of items.</summary>
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

		/// <summary>Always <c>false</c>.</summary>
		public Boolean IsReadOnly
		{ get { return false; } }

		/// <summary>Adds an item.</summary>
		/// <param name="item">Item to add.</param>
		public void Add(T item)
		{
			lock (_lock)
			{
				_items.Add(item);
			}
		}

		/// <summary>Removes an item.</summary>
		/// <param name="item">Item to remove.</param>
		/// <returns><c>true</c> if the item was present.</returns>
		public Boolean Remove(T item)
		{
			lock (_lock)
			{
				return _items.Remove(item);
			}
		}

		/// <summary>Removes every item.</summary>
		public void Clear()
		{
			lock (_lock)
			{
				_items.Clear();
			}
		}

		/// <summary>Determines whether an item is present.</summary>
		/// <param name="item">Item to look for.</param>
		/// <returns><c>true</c> if the item is present.</returns>
		public Boolean Contains(T item)
		{
			lock (_lock)
			{
				return _items.Contains(item);
			}
		}

		/// <summary>Copies the items into an array.</summary>
		/// <param name="array">Destination array.</param>
		/// <param name="arrayIndex">Index to start writing at.</param>
		public void CopyTo(T[] array, Int32 arrayIndex)
		{
			lock (_lock)
			{
				_items.CopyTo(array, arrayIndex);
			}
		}

		/// <summary>Enumerates a snapshot taken at the moment of the call.</summary>
		/// <returns>An enumerator over the snapshot.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return CreateCopy().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		/// <summary>Takes a snapshot of the current items.</summary>
		/// <returns>A new list holding the items.</returns>
		public List<T> CreateCopy()
		{
			lock (_lock)
			{
				return new List<T>(_items);
			}
		}
	}
}
