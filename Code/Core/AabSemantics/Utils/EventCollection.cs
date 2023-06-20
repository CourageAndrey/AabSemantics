using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Utils
{
	public abstract class EventCollectionBase<T> : IEventCollection<T>
	{
		#region Properties

		public event EventHandler<ItemEventArgs<T>> ItemAdded;

		public event EventHandler<ItemEventArgs<T>> ItemRemoved;

		public event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		public event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;

		#endregion

		#region Implementation of ICollection<T>

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Boolean IsReadOnly
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
			AddImplementation(item);
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
				var copy = new List<T>(this);
				ClearImplementation();
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

		public Boolean Remove(T item)
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
			Boolean result = RemoveImplementation(item);
			Volatile.Read(ref ItemRemoved)?.Invoke(this, new ItemEventArgs<T>(item));
			return result;
		}

		#endregion

		#region Methods to override

		public abstract IEnumerator<T> GetEnumerator();

		public abstract Int32 Count
		{ get; }

		protected abstract void AddImplementation(T item);

		protected abstract void ClearImplementation();

		public abstract Boolean Contains(T item);

		public abstract void CopyTo(T[] array, Int32 arrayIndex);

		protected abstract Boolean RemoveImplementation(T item);

		#endregion
	}

	public class EventCollection<T> : EventCollectionBase<T>, IKeyedCollection<T>
		where T : IIdentifiable
	{
		#region Properties

		public T this[String id]
		{ get { return _collection[id]; } }

		public ICollection<String> Keys
		{ get { return _collection.Keys; } }

		private readonly IDictionary<String, T> _collection;

		#endregion

		#region Implementation of EventCollectionBase<T>

		public override IEnumerator<T> GetEnumerator()
		{
			return _collection.Values.GetEnumerator();
		}

		public override Int32 Count
		{ get { return _collection.Count; } }

		protected override void AddImplementation(T item)
		{
			_collection.Add(item.ID, item);
		}

		protected override void ClearImplementation()
		{
			_collection.Clear();
		}

		public override Boolean Contains(T item)
		{
			return _collection.Values.Contains(item);
		}

		public override void CopyTo(T[] array, Int32 arrayIndex)
		{
			_collection.Values.CopyTo(array, arrayIndex);
		}

		protected override Boolean RemoveImplementation(T item)
		{
			return _collection.Remove(item.ID);
		}

		#endregion

		#region Implementation of IKeyedCollection<T>

		public Boolean TryGetValue(String key, out T value)
		{
			return _collection.TryGetValue(key, out value);
		}

		#endregion

		#region Constructors

		public EventCollection()
			: this(Array.Empty<T>())
		{ }

		public EventCollection(IEnumerable<T> items)
			: this(items.ToDictionary(i => i.ID, i => i))
		{ }

		public EventCollection(IDictionary<String, T> items)
		{
			_collection = items;
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
		public Boolean IsCanceled
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
		public ItemsCantBeRemovedException(IEnumerable<T> items)
			: base("Some items can not be removed.")
		{
			Items = new List<T>(items.EnsureNotNull(nameof(items)));
		}

		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="info">serialization info</param>
		/// <param name="context">streaming context</param>
		public ItemsCantBeRemovedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			String itemsString = info.GetString("items");

			Items = itemsString.DeserializeFromXmlString<SerializationWrapper>().Items;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			var wrapper = new SerializationWrapper(Items);

			info.AddValue("items", wrapper.SerializeToXmlString());
		}

		[XmlType]
		public class SerializationWrapper
		{
			[XmlArray(nameof(Items))]
			[XmlArrayItem("Item")]
			public List<T> Items
			{ get; }

			public SerializationWrapper(IEnumerable<T> items)
			{
				Items = new List<T>(items);
			}

			public SerializationWrapper()
				: this(Array.Empty<T>())
			{ }
		}
	}

	#endregion
}
