using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Inventor.Core
{
	public class EventList<T> : IList<T>
	{
		#region Events

		public delegate void ItemDelegate(IList<T> list, T item);
		public delegate void ItemQueryDelegate(IList<T> list, T item, ref bool allowed);

		public event ItemDelegate Added;
		public event ItemDelegate Removed;
		public event ItemQueryDelegate Adding;
		public event ItemQueryDelegate Removing;
		public event EventHandler Changed;

		private void raiseAdded(T item)
		{
			var handler = Volatile.Read(ref Added);
			if (handler != null)
			{
				handler(this, item);
			}
		}

		private void raiseRemoved(T item)
		{
			var handler = Volatile.Read(ref Removed);
			if (handler != null)
			{
				handler(this, item);
			}
		}

		private Boolean raiseAdding(T item)
		{
			var handler = Volatile.Read(ref Adding);
			bool allowed = true;
			if (handler != null)
			{
				handler(this, item, ref allowed);
			}
			return allowed;
		}

		private Boolean raiseRemoving(T item)
		{
			var handler = Volatile.Read(ref Removing);
			bool allowed = true;
			if (handler != null)
			{
				handler(this, item, ref allowed);
			}
			return allowed;
		}

		private void raiseChanged()
		{
			var handler = Volatile.Read(ref Changed);
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Properties

		private readonly List<T> innerList = new List<T>();

		#endregion

		#region Constructors

		public EventList(
			ItemDelegate added = null, ItemDelegate removed = null,
			ItemQueryDelegate adding = null, ItemQueryDelegate removing = null,
			EventHandler changed = null)
		{
			Added = added;
			Removed = removed;
			Adding = adding;
			Removing = removing;
			Changed = changed;
		}

		public EventList(
			IEnumerable<T> otherList,
			ItemDelegate added = null, ItemDelegate removed = null,
			ItemQueryDelegate adding = null, ItemQueryDelegate removing = null,
			EventHandler changed = null) : this(added, removed, adding, removing, changed)
		{
			innerList.AddRange(otherList);
		}

		#endregion

		#region Implementation of IList<T>

		public IEnumerator<T> GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			if (raiseAdding(item))
			{
				innerList.Add(item);
				raiseAdded(item);
				raiseChanged();
			}
		}

		public void Clear()
		{
			foreach (var item in new List<T>(innerList))
			{
				Remove(item);
			}
		}

		public bool Contains(T item)
		{
			return innerList.Contains(item);
		}

		public void CopyTo(T[] array, Int32 arrayIndex)
		{
			innerList.CopyTo(array, arrayIndex);
		}

		public Boolean Remove(T item)
		{
			if (raiseRemoving(item))
			{
				innerList.Remove(item);
				raiseRemoved(item);
				raiseChanged();
				return true;
			}
			else
			{
				return false;
			}
		}

		public Int32 Count
		{ get { return innerList.Count; } }

		public Boolean IsReadOnly
		{ get { return false; } }

		public Int32 IndexOf(T item)
		{
			return innerList.IndexOf(item);
		}

		public void Insert(Int32 index, T item)
		{
			if (raiseAdding(item))
			{
				innerList.Insert(index, item);
				raiseAdded(item);
				raiseChanged();
			}
		}

		public void RemoveAt(Int32 index)
		{
			Remove(innerList[index]);
		}

		public T this[Int32 index]
		{
			get { return innerList[index]; }
			set
			{
				if (raiseRemoving(innerList[index]) && raiseAdding(value))
				{
					raiseRemoved(innerList[index]);
					innerList[index] = value;
					raiseAdded(value);
					raiseChanged();
				}
			}
		}

		#endregion
	}
}
