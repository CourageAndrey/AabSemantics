using System.Collections.Generic;
using System.Text;

namespace Inventor.Core.Localization
{
    public interface INamed
    {
        LocalizedString Name
        { get; }
    }

    public static class NamedHelper
    {
        public static Dictionary<string, INamed> Enumerate(this IList<INamed> list, out string format)
        {
            var formatText = new StringBuilder();
            var paremeters = new Dictionary<string, INamed>();
            for (int i = 0; i < list.Count; i++)
            {
                string param = string.Format("#ENUMITEM{0:00000000}#", i);
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

        public static Dictionary<string, INamed> Enumerate<T>(this IList<T> list, out string format)
            where T : INamed
        {
            var formatText = new StringBuilder();
            var paremeters = new Dictionary<string, INamed>();
            for (int i = 0; i < list.Count; i++)
            {
                string param = string.Format("#ENUMITEM{0:00000000}#", i);
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
    }
}
