using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// A collection that reports its own modifications, both before and after they happen.
	/// The semantic network uses these events to keep concepts and statements consistent —
	/// for example, removing a concept also removes the statements referring to it.
	/// </summary>
	/// <typeparam name="T">Type of the stored items.</typeparam>
	public interface IEventCollection<T> : IEnumerable<T>
	{
		/// <summary>
		/// Raised after an item has been added.
		/// </summary>
		event EventHandler<ItemEventArgs<T>> ItemAdded;

		/// <summary>
		/// Raised after an item has been removed.
		/// </summary>
		event EventHandler<ItemEventArgs<T>> ItemRemoved;

		/// <summary>
		/// Raised before an item is added; a handler can cancel the addition.
		/// </summary>
		event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		/// <summary>
		/// Raised before an item is removed; a handler can cancel the removal.
		/// </summary>
		event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;
	}
}
