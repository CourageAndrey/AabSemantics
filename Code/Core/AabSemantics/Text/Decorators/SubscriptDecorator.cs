namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text as subscript.</summary>
	public class SubscriptDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render as subscript.</param>
		public SubscriptDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
