using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Text
{
	/// <summary>Base of text nodes holding an ordered sequence of nested nodes.</summary>
	public abstract class TextContainerBase : TextBase, ITextContainer
	{
		/// <summary>Nested texts, in rendering order.</summary>
		public IList<IText> Items
		{ get; }

		/// <summary>Wraps a list of text nodes, using it directly rather than copying it.</summary>
		/// <param name="items">Nested texts.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="items"/> is <c>null</c>.</exception>
		protected TextContainerBase(IList<IText> items)
		{
			Items = items.EnsureNotNull(nameof(items));
		}

		/// <summary>
		/// Merges the references of every nested text. Note that entries are re-keyed by item
		/// identifier rather than by the child's anchor token.
		/// </summary>
		/// <returns>Item identifier to referenced item.</returns>
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			var result = new Dictionary<String, IKnowledge>();
			foreach (var text in Items)
			{
				foreach (var parameter in text.GetParameters())
				{
					result[parameter.Value.ID] = parameter.Value;
				}
			}
			return result;
		}
	}
}