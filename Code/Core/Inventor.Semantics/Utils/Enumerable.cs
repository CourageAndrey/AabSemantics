using System;
using System.Collections.Generic;

namespace Inventor.Semantics.Utils
{
	public static class Enumerable
	{
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
