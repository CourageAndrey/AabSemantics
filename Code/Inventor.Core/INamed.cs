using System;
using System.Collections.Generic;
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
		public static Dictionary<String , INamed> Enumerate(this IList<INamed> list, out String format)
		{
			var formatText = new StringBuilder();
			var paremeters = new Dictionary<String , INamed>();
			for (Int32 i = 0; i < list.Count; i++)
			{
				String param = String .Format("#ENUMITEM{0:00000000}#", i);
				paremeters[param] = list[i];
				formatText.Append(param);
				if (i != list.Count - 1)
				{
					formatText.Append(", ");
				}
			}
			format = formatText.ToString();
			return paremeters;
		}

		public static Dictionary<String , INamed> Enumerate<T>(this IEnumerable<T> amount, out String format)
			where T : INamed
		{
			var formatText = new StringBuilder();
			var paremeters = new Dictionary<String , INamed>();
			Int32 index = 0;
			foreach (var item in amount)
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
	}
}
