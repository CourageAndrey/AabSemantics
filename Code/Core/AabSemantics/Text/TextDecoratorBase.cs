using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Text
{
	/// <summary>Base of text nodes that wrap exactly one other node to change its presentation.</summary>
	public abstract class TextDecoratorBase : TextBase, ITextDecorator
	{
		/// <summary>The wrapped text.</summary>
		public IText InnerText
		{ get; }

		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to wrap.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="innerText"/> is <c>null</c>.</exception>
		protected TextDecoratorBase(IText innerText)
		{
			InnerText = innerText.EnsureNotNull(nameof(innerText));
		}

		/// <summary>Passes through the wrapped text's references; a decorator adds none of its own.</summary>
		/// <returns>Anchor token to referenced item.</returns>
		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return InnerText.GetParameters();
		}
	}
}