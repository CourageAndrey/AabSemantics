namespace AabSemantics.Text.Decorators
{
	/// <summary>Renders the wrapped text struck out.</summary>
	public class StrikeoutDecorator : TextDecoratorBase
	{
		/// <summary>Wraps a text node.</summary>
		/// <param name="innerText">Text to render struck out.</param>
		public StrikeoutDecorator(IText innerText)
			: base(innerText)
		{ }
	}
}
