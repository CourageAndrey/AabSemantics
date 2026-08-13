namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text as its own paragraph.</summary>
	public class ParagraphDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render as a paragraph.</param>
		public ParagraphDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}