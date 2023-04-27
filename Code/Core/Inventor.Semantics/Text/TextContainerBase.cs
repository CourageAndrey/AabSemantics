using System;
using System.Collections.Generic;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Text
{
	public abstract class TextContainerBase : TextBase, ITextContainer
	{
		public IList<IText> Items
		{ get; }

		protected TextContainerBase(IList<IText> items)
		{
			Items = items.EnsureNotNull(nameof(items));
		}

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