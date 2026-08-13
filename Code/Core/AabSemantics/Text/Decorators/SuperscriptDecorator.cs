namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text as superscript.</summary>
	public class SuperscriptDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render as superscript.</param>
		public SuperscriptDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
