using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Text;

namespace Inventor.Core
{
	public interface IKnowledge : INamed, IIdentifiable
	{
		ILocalizedString Hint
		{ get; }
	}

	public static class KnowledgeHelper
	{
		public static String GetAnchor(this IKnowledge knowledge)
		{
			return $"#{knowledge.ID}#";
		}

		public static List<IText> Enumerate(this IEnumerable<IKnowledge> knowledgeItems)
		{
			return knowledgeItems.Select(k => new FormattedText(
				l => k.GetAnchor(),
				new Dictionary<String, IKnowledge> { { k.GetAnchor(), k } }) as IText).ToList();
		}

		public static IText EnumerateOneLine(this IEnumerable<IKnowledge> knowledgeItems)
		{
			var parameters = knowledgeItems.ToDictionary(
				k => k.GetAnchor(),
				k => k);
			String format = String.Join(", ", parameters.Keys);
			return new FormattedText(language => format, parameters);
		}
	}
}
