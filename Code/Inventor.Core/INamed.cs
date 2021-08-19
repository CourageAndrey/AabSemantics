using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventor.Core
{
	public interface INamed
	{
		ILocalizedString Name
		{ get; }
	}

	public static class NamedHelper
	{
		public static Dictionary<String , INamed> Enumerate(this IEnumerable<INamed> namedItems, out String format)
		{
			var formatText = new StringBuilder();
			var paremeters = new Dictionary<String , INamed>();
			Int32 index = 0;
			foreach (var item in namedItems)
			{
				if (index > 0)
				{
					formatText.Append(", ");
				}
				String param = String .Format("#ENUMITEM{0:00000000}#", index);
				paremeters[param] = item;
				formatText.Append(param);
				index++;
			}
			format = formatText.ToString();
			return paremeters;
		}

		public static Dictionary<String , INamed> Enumerate<T>(this IEnumerable<T> namedItems, out String format)
			where T : INamed
		{
			return namedItems.OfType<INamed>().Enumerate(out format);
		}
	}
}
