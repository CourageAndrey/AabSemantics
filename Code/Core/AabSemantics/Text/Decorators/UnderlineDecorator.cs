namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text underlined.</summary>
	public class UnderlineDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render underlined.</param>
		public UnderlineDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
