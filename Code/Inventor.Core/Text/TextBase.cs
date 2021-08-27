using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Text
{
	public abstract class TextBase : IText
	{
		public sealed override String ToString()
		{
			return TextRepresenters.PlainString.Represent(this, Language.Default).ToString();
		}

		public abstract IDictionary<String, IKnowledge> GetParameters();
	}
}
