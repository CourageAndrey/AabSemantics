using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Utils
{
	public class Repository<T> : IRepository<T>, IEventCollection<T>, ICollection<T>
		where T : IIdentifiable
	{
		private readonly IDictionary<String, T> _collection;

		#region Implementation of IEventCollection<T>

		public event EventHandler<ItemEventArgs<T>> ItemAdded;

		public event EventHandler<ItemEventArgs<T>> ItemRemoved;

		public event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		public event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;

		#endregion

		#region Implementation of IEnumerable<T>

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _collection.Values.GetEnumerator();
		}

		#endregion

		#region Implementation of IRepository<T>

		public async Task AddAsync(T item)
		{
			Add(item);
			await Task.CompletedTask;
		}

		public async Task<Boolean> RemoveAsync(T item)
		{
			return await Task.FromResult(Remove(item));
		}

		public async Task ClearAsync()
		{
			Clear();
			await Task.CompletedTask;
		}

		public async Task<Int32> GetCountAsync()
		{
			return await Task.FromResult(Count);
		}

		public async Task<T> GetItemAsync(String key)
		{
			return await Task.FromResult(this[key]);
		}

		public async Task<ICollection<String>> GetKeysAsync()
		{
			return await Task.FromResult(Keys);
		}

		public async Task<Boolean> ContainsAsync(String key)
		{
			return await Task.FromResult(Contains(key));
		}

		public async Task<KeyValuePair<Boolean, T>> TryGetValueAsync(String key)
		{
			T result;
			Boolean found = _collection.TryGetValue(key, out result);
			return await Task.FromResult(new KeyValuePair<Boolean, T>(
				found,
				found ? result : default(T)));
		}

		#endregion

		#region Sync API

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

			_collection.Add(item.ID, item);

			Volatile.Read(ref ItemAdded)?.Invoke(this, new ItemEventArgs<T>(item));
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

			Boolean result = _collection.Remove(item.ID);

			Volatile.Read(ref ItemRemoved)?.Invoke(this, new ItemEventArgs<T>(item));
			return result;
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

				_collection.Clear();

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

		public Int32 Count
		{ get { return _collection.Count; } }

		public T this[String id]
		{ get { return _collection[id]; } }

		public ICollection<String> Keys
		{ get { return _collection.Keys; } }

		public Boolean Contains(String key)
		{
			return _collection.ContainsKey(key);
		}

		public Boolean TryGetValue(String key, out T value)
		{
			return _collection.TryGetValue(key, out value);
		}

		#endregion

		#region Implementation of ICollection<T>

		public Boolean IsReadOnly
		{ get { return false; } }

		public Boolean Contains(T item)
		{
			return _collection.Values.Contains(item);
		}

		public void CopyTo(T[] array, Int32 arrayIndex)
		{
			_collection.Values.CopyTo(array, arrayIndex);
		}

		#endregion

		#region Constructors

		public Repository()
			: this(Array.Empty<T>())
		{ }

		public Repository(IEnumerable<T> items)
			: this(items.ToDictionary(i => i.ID, i => i))
		{ }

		public Repository(IDictionary<String, T> items)
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
