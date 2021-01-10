using System;
using System.Collections.Generic;

using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface IEventCollection<T> : ICollection<T>
	{
		event EventHandler<ItemEventArgs<T>> ItemAdded;

		event EventHandler<ItemEventArgs<T>> ItemRemoved;

		event EventHandler<CancelableItemEventArgs<T>> ItemAdding;

		event EventHandler<CancelableItemEventArgs<T>> ItemRemoving;
	}
}
