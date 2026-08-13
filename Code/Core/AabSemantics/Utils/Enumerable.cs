using System;
using System.Collections.Generic;

namespace AabSemantics.Utils
{
	/// <summary>Sequence helpers missing from LINQ.</summary>
	public static class Enumerable
	{
		/// <summary>Finds an item's position in a sequence, comparing by reference.</summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="sequence">Sequence to scan.</param>
		/// <param name="item">Item to look for.</param>
		/// <returns>Zero-based position, or <c>-1</c> when not found.</returns>
		public static Int32 IndexOf<T>(this IEnumerable<T> sequence, T item)
			where T : class
		{
			Int32 index = 0;

			foreach (var i in sequence)
			{
				if (i == item)
				{
					return index;
				}
				else
				{
					index++;
				}
			}

			return -1;
		}
	}
}
