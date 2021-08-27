using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface INamed
	{
		ILocalizedString Name
		{ get; }
	}

	public static class NamedHelper
	{
		public static Dictionary<String, IKnowledge> Enumerate(this IEnumerable<IKnowledge> knowledgeItems, out String format)
		{
			var parameters = knowledgeItems.ToDictionary(
				k => $"#{k.ID}#",
				k => k);
			format = String.Join(", ", parameters.Keys);
			return parameters;
		}
	}
}
