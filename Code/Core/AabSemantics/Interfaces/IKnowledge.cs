using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Text.Primitives;

namespace AabSemantics
{
	/// <summary>
	/// A single piece of knowledge stored in a semantic network: a concept or a statement.
	/// Being both named and identifiable, it can be displayed to the user and referenced from generated text.
	/// </summary>
	public interface IKnowledge : INamed, IIdentifiable
	{
		/// <summary>
		/// Optional localized explanation of the item, suitable for tooltips.
		/// </summary>
		ILocalizedString Hint
		{ get; }
	}

	/// <summary>
	/// Helpers turning knowledge items into <see cref="IText"/> that keeps them referenceable.
	/// </summary>
	public static class KnowledgeHelper
	{
		/// <summary>
		/// Returns the <c>#ID#</c> token that represents the item inside a text template.
		/// Renders resolve such tokens back into links to the item.
		/// </summary>
		/// <param name="knowledge">Item to build an anchor for.</param>
		/// <returns>Anchor token, e.g. <c>#1234#</c>.</returns>
		public static String GetAnchor(this IKnowledge knowledge)
		{
			return $"#{knowledge.ID}#";
		}

		/// <summary>
		/// Renders each item as its own text line, so that a caller can present them as a list.
		/// </summary>
		/// <param name="knowledgeItems">Items to render.</param>
		/// <returns>One <see cref="IText"/> per item, in the original order.</returns>
		public static List<IText> Enumerate(this IEnumerable<IKnowledge> knowledgeItems)
		{
			return knowledgeItems.Select(k => new FormattedText(
				l => k.GetAnchor(),
				new Dictionary<String, IKnowledge> { { k.GetAnchor(), k } }) as IText).ToList();
		}

		/// <summary>
		/// Renders all items as a single comma-separated text.
		/// </summary>
		/// <param name="knowledgeItems">Items to render. Items sharing an anchor are collapsed into one.</param>
		/// <returns>A single <see cref="IText"/> listing every item.</returns>
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
