using System;

using Inventor.Core.Localization;

namespace Inventor.Core.Text
{
	public class TextBase : IText
	{
		public sealed override String ToString()
		{
			return TextRepresenters.PlainString.Represent(this, Language.Default).ToString();
		}
	}
}
