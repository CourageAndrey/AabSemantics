namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text in italic.</summary>
	public class ItalicDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render in italic.</param>
		public ItalicDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
