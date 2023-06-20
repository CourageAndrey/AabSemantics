using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Text
{
	public abstract class TextDecoratorBase : TextBase, ITextDecorator
	{
		public IText InnerText
		{ get; }

		protected TextDecoratorBase(IText innerText)
		{
			InnerText = innerText.EnsureNotNull(nameof(innerText));
		}

		public override IDictionary<String, IKnowledge> GetParameters()
		{
			return InnerText.GetParameters();
		}
	}
}