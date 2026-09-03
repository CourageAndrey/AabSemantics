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
	/// <summary>
	/// In-memory <see cref="IRepository{T}"/> keyed by item identifier, raising events around
	/// every modification. The asynchronous methods complete synchronously and have nothing to wait
	/// for, so they observe the cancellation token before they start and then run to completion;
	/// the collection itself is not thread-safe.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class Repository<T> : IRepository<T>, IEventCollection<T>, ICollection<T>
		where T : IIdentifiable
	{
		private readonly IDictionary<String, T> _collection;

		#region Implementation of IEventCollection<T>

		/// <summary>Raised after an item has been added.</summary>
		public event EventHandler<ItemEventArgs<T>> ItemAdded;

		/// <summary>Raised after an item has been removed.</summary>
		public event EventHandler<ItemEventArgs<T>> ItemRemoved;

		/// <summary>Raised before an item is added; a handler can cancel the addition.</summary>
		public event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		/// <summary>Raised before an item is removed; a handler can cancel the removal.</summary>
		public event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;

		#endregion

		#region Implementation of IEnumerable<T>

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>Enumerates the stored items.</summary>
		/// <returns>An enumerator over the live collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return _collection.Values.GetEnumerator();
		}

		#endregion

		#region Implementation of IRepository<T>

		/// <summary>Stores an item.</summary>
		/// <param name="item">Item to store.</param>
		/// <param name="cancellationToken">Cancels the call before the item is stored.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task AddAsync(T item, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Add(item), cancellationToken);
		}

		/// <summary>Removes an item.</summary>
		/// <param name="item">Item to remove.</param>
		/// <param name="cancellationToken">Cancels the call before the item is removed.</param>
		/// <returns><c>true</c> if the item was present and has been removed.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> RemoveAsync(T item, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Remove(item), cancellationToken);
		}

		/// <summary>Removes every item.</summary>
		/// <param name="cancellationToken">Cancels the call before anything is removed.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task ClearAsync(CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Clear(), cancellationToken);
		}

		/// <summary>Counts the stored items.</summary>
		/// <param name="cancellationToken">Cancels the call before the items are counted.</param>
		/// <returns>Number of items.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Int32> GetCountAsync(CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Count, cancellationToken);
		}

		/// <summary>Looks an item up by key.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels the call before the item is looked up.</param>
		/// <returns>The matching item.</returns>
		/// <exception cref="KeyNotFoundException">No item has that identifier.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<T> GetItemAsync(String key, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => this[key], cancellationToken);
		}

		/// <summary>Lists the identifiers of every stored item.</summary>
		/// <param name="cancellationToken">Cancels the call before the keys are listed.</param>
		/// <returns>All keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<String>> GetKeysAsync(CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Keys, cancellationToken);
		}

		/// <summary>Determines whether an item with the given key is stored.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels the call before the key is looked for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> ContainsAsync(String key, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => Contains(key), cancellationToken);
		}

		/// <summary>Looks an item up without throwing when it is absent.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="cancellationToken">Cancels the call before the item is looked up.</param>
		/// <returns>A pair whose key reports success and whose value holds the item.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<KeyValuePair<Boolean, T>> TryGetValueAsync(String key, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(
				() =>
				{
					T result;
					Boolean found = _collection.TryGetValue(key, out result);
					return new KeyValuePair<Boolean, T>(
						found,
						found ? result : default(T));
				},
				cancellationToken);
		}

		#endregion

		#region Sync API

		/// <summary>Stores an item, unless an <see cref="ItemAdding"/> handler cancels it.</summary>
		/// <param name="item">Item to store; its identifier becomes its key.</param>
		/// <exception cref="ArgumentException">An item with the same identifier is already stored.</exception>
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

		/// <summary>Removes an item, unless an <see cref="ItemRemoving"/> handler cancels it.</summary>
		/// <param name="item">Item to remove; matched by identifier.</param>
		/// <returns><c>true</c> if the item was present; <c>false</c> if it was absent or the removal was cancelled.</returns>
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

		/// <summary>
		/// Removes every item, but only if all of them may go: the operation is all-or-nothing,
		/// so a single veto from an <see cref="ItemRemoving"/> handler leaves the repository untouched.
		/// </summary>
		/// <exception cref="ItemsCantBeRemovedException{T}">At least one removal was cancelled.</exception>
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

		/// <summary>Number of stored items.</summary>
		public Int32 Count
		{ get { return _collection.Count; } }

		/// <summary>Looks an item up by identifier.</summary>
		/// <param name="id">Identifier of the wanted item.</param>
		/// <returns>The matching item.</returns>
		/// <exception cref="KeyNotFoundException">No item has that identifier.</exception>
		public T this[String id]
		{ get { return _collection[id]; } }

		/// <summary>Identifiers of every stored item.</summary>
		public ICollection<String> Keys
		{ get { return _collection.Keys; } }

		/// <summary>Determines whether an item with the given identifier is stored.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <returns><c>true</c> if such an item exists.</returns>
		public Boolean Contains(String key)
		{
			return _collection.ContainsKey(key);
		}

		/// <summary>Looks an item up without throwing when it is absent.</summary>
		/// <param name="key">Identifier of the wanted item.</param>
		/// <param name="value">Receives the matching item, or the type's default.</param>
		/// <returns><c>true</c> if an item was found.</returns>
		public Boolean TryGetValue(String key, out T value)
		{
			return _collection.TryGetValue(key, out value);
		}

		#endregion

		#region Implementation of ICollection<T>

		/// <summary>Always <c>false</c>.</summary>
		public Boolean IsReadOnly
		{ get { return false; } }

		/// <summary>Determines whether an item is stored, comparing the item itself rather than its identifier.</summary>
		/// <param name="item">Item to look for.</param>
		/// <returns><c>true</c> if the item is stored.</returns>
		public Boolean Contains(T item)
		{
			return _collection.Values.Contains(item);
		}

		/// <summary>Copies the stored items into an array.</summary>
		/// <param name="array">Destination array.</param>
		/// <param name="arrayIndex">Index to start writing at.</param>
		public void CopyTo(T[] array, Int32 arrayIndex)
		{
			_collection.Values.CopyTo(array, arrayIndex);
		}

		#endregion

		#region Constructors

		/// <summary>Creates an empty repository.</summary>
		public Repository()
			: this(Array.Empty<T>())
		{ }

		/// <summary>Creates a repository holding the given items, keyed by their identifiers.</summary>
		/// <param name="items">Items to store; their identifiers must be unique.</param>
		/// <exception cref="ArgumentException">Two items share an identifier.</exception>
		public Repository(IEnumerable<T> items)
			: this(items.ToDictionary(i => i.ID, i => i))
		{ }

		/// <summary>Wraps an existing dictionary, which is used as the backing store rather than copied.</summary>
		/// <param name="items">Dictionary of items keyed by identifier.</param>
		public Repository(IDictionary<String, T> items)
		{
			_collection = items;
		}

		#endregion
	}

	#region Support classes

	/// <summary>Arguments of an event reporting a change to a single item.</summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class ItemEventArgs<T>
	{
		/// <summary>The item the event is about.</summary>
		public T Item
		{ get; }

		/// <summary>Creates the event arguments.</summary>
		/// <param name="item">The item the event is about.</param>
		public ItemEventArgs(T item)
		{
			Item = item;
		}
	}

	/// <summary>Arguments of a pre-change event, which a handler can veto.</summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class CancelableItemEventArgs<T> : ItemEventArgs<T>
	{
		/// <summary>Set by a handler to abort the pending change.</summary>
		public Boolean IsCanceled
		{ get; set; }

		/// <summary>Creates the event arguments, not cancelled.</summary>
		/// <param name="item">The item the event is about.</param>
		public CancelableItemEventArgs(T item)
			: base(item)
		{
			IsCanceled = false;
		}
	}

	/// <summary>
	/// Exception, which declares that it is impossible to remove some items from collection.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
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

		/// <summary>Writes the blocked items into the serialization stream as XML.</summary>
		/// <param name="info">Serialization info to populate.</param>
		/// <param name="context">Streaming context.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			var wrapper = new SerializationWrapper(Items);

			info.AddValue("items", wrapper.SerializeToXmlString());
		}

		/// <summary>XML surrogate carrying the blocked items through serialization.</summary>
		[XmlType]
		public class SerializationWrapper
		{
			/// <summary>The wrapped items.</summary>
			[XmlArray(nameof(Items))]
			[XmlArrayItem("Item")]
			public List<T> Items
			{ get; }

			/// <summary>Creates a wrapper around the given items.</summary>
			/// <param name="items">Items to wrap; copied into the wrapper.</param>
			public SerializationWrapper(IEnumerable<T> items)
			{
				Items = new List<T>(items);
			}

			/// <summary>Creates an empty wrapper, as required by the XML serializer.</summary>
			public SerializationWrapper()
				: this(Array.Empty<T>())
			{ }
		}
	}

	#endregion
}
