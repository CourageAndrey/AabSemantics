using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics
{
	public interface IEventCollection<T> : IEnumerable<T>
	{
		event EventHandler<ItemEventArgs<T>> ItemAdded;

		event EventHandler<ItemEventArgs<T>> ItemRemoved;

		event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;
	}
}
