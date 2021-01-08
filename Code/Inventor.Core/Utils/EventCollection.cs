using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Inventor.Core
{
	public class EventCollection<T> : ICollection<T>
	{
		#region Properties

		private readonly ICollection<T> collection = new List<T>();

		public event EventHandler<ItemEventArgs<T>> ItemAdded;

		public event EventHandler<ItemEventArgs<T>> ItemRemoved;

		public event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		public event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;

		#endregion

		#region Implementation of ICollection<IKnowledge>

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		public int Count
		{ get { return collection.Count; } }

		public bool IsReadOnly
		{ get { return false; } }

		public void Add(T item)
		{
			var beforeHandler = Volatile.Read(ref ItemAdding);
			if (beforeHandler != null)
			{
				var eventArgs = new CancelableItemEventArgs<T>(item);
				beforeHandler(this, eventArgs);
				if (eventArgs.IsCanceled)
				{
					return;
				}
			}
			collection.Add(item);
			Volatile.Read(ref ItemAdded)?.Invoke(this, new ItemEventArgs<T>(item));
		}

		public void Clear()
		{
			var itemsWhichCanNotBeRemoved = new List<T>();
			var beforeHandler = Volatile.Read(ref ItemRemoving);
			foreach (var item in this)
			{
				if (beforeHandler != null)
				{
					var eventArgs = new CancelableItemEventArgs<T>(item);
					beforeHandler(this, eventArgs);
					if (eventArgs.IsCanceled)
					{
						itemsWhichCanNotBeRemoved.Add(item);
					}
				}
			}
			if (itemsWhichCanNotBeRemoved.Count == 0)
			{
				var copy = new List<T>(collection);
				collection.Clear();
				var afterHandler = Volatile.Read(ref ItemRemoved);
				foreach (var item in copy)
				{
					afterHandler?.Invoke(this, new ItemEventArgs<T>(item));
				}
			}
			else
			{
				throw new ItemsCantBeRemovedException<T>(itemsWhichCanNotBeRemoved);
			}
		}

		public bool Contains(T item)
		{
			return collection.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			var beforeHandler = Volatile.Read(ref ItemRemoving);
			if (beforeHandler != null)
			{
				var eventArgs = new CancelableItemEventArgs<T>(item);
				beforeHandler(this, eventArgs);
				if (eventArgs.IsCanceled)
				{
					return false;
				}
			}
			bool result = collection.Remove(item);
			Volatile.Read(ref ItemRemoved)?.Invoke(this, new ItemEventArgs<T>(item));
			return result;
		}

		#endregion
	}

	#region Support classes

	public class ItemEventArgs<T>
	{
		public T Item
		{ get; }

		public ItemEventArgs(T item)
		{
			Item = item;
		}
	}

	public class CancelableItemEventArgs<T> : ItemEventArgs<T>
	{
		public bool IsCanceled
		{ get; set; }

		public CancelableItemEventArgs(T item)
			: base(item)
		{
			IsCanceled = false;
		}
	}

	/// <summary>
	/// Exception, which declares that it is impossible to remove some items from collection.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class ItemsCantBeRemovedException<T> : Exception
	{
		/// <summary>
		/// Collection of items.
		/// </summary>
		public ICollection<T> Items
		{ get; }

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="items">items</param>
		public ItemsCantBeRemovedException(ICollection<T> items)
			: base("Some items can not be removed.")
		{
			Items = items;
		}

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="info">serialization info</param>
		/// <param name="context">streaming context</param>
		protected ItemsCantBeRemovedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	#endregion
}
