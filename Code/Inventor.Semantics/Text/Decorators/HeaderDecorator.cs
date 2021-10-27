using System;

namespace Inventor.Semantics.Text.Decorators
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
