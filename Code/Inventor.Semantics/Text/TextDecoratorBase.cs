using System;
using System.Collections.Generic;

namespace Inventor.Semantics.Text
{
	public abstract class TextDecoratorBase : TextBase, ITextDecorator
	{
		public IText InnerText
		{ get; }

		protected TextDecoratorBase(IText innerText)
		{
			if (innerText == null) throw new ArgumentNullException(nameof(innerText));

			InnerText = innerText;
		}

		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return InnerText.GetParameters();
		}
	}
}