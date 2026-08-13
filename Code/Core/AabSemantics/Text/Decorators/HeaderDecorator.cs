using System;

namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text as a heading.</summary>
	public class HeaderDecorator : TextDecoratorBase
	{
		/// <summary>Heading level, as in HTML's <c>h1</c>..<c>h6</c>.</summary>
		public Byte Level
		{ get; }

		/// <summary>Wraps a text node as a heading.</summary>
		/// <param name="innerText">Text to render as a heading.</param>
		/// <param name="level">Heading level; not validated against any range.</param>
		public HeaderDecorator(IText innerText, Byte level)
			: base(innerText)
		{
			Level = level;
		}
	}
}
