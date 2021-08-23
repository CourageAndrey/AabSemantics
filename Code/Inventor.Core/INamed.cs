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
		public static Dictionary<String , INamed> Enumerate(this IEnumerable<IKnowledge> knowledgeItems, out String format)
		{
			var parameters = knowledgeItems.ToDictionary(
				k => $"#{k.ID}#",
				k => k as INamed);
			format = String.Join(", ", parameters.Keys);
			return parameters;
		}

		public static Dictionary<String , INamed> Enumerate<T>(this IEnumerable<T> knowledgeItems, out String format)
			where T : IKnowledge
		{
			return knowledgeItems.OfType<IKnowledge>().Enumerate(out format);
		}
	}
}
