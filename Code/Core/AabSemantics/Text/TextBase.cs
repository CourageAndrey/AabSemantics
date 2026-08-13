using System;
using System.Collections.Generic;

using AabSemantics.Localization;

namespace AabSemantics.Text
{
	/// <summary>Base of every text node.</summary>
	public abstract class TextBase : IText
	{
		/// <summary>Renders the node as plain text in the default language, for diagnostics.</summary>
		/// <returns>The rendered string.</returns>
		public sealed override String ToString()
		{
			return TextRenders.PlainString.RenderText(this, Language.Default).ToString();
		}

		/// <summary>Collects the knowledge items this node and its children reference.</summary>
		/// <returns>Anchor token to referenced item.</returns>
		public abstract IDictionary<String, IKnowledge> GetParameters();
	}
}
