using System;

namespace AabSemantics.Text.Decorators
{
	public class HeaderDecorator : TextDecoratorBase
	{
		public Byte Level
		{ get; }

		public HeaderDecorator(IText innerText, Byte level)
			: base(innerText)
		{
			Level = level;
		}
	}
}
