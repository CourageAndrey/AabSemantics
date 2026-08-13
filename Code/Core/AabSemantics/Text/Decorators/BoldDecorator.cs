namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text in bold.</summary>
	public class BoldDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render in bold.</param>
		public BoldDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
