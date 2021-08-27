using System;
using System.Text;

namespace Inventor.Core.Text
{
	public class PlainStringTextRepresenter : ITextRepresenter<StringBuilder>
	{
		public StringBuilder Represent(IText text, ILanguage language)
		{
			if (text is TextBlock)
			{
				return representTextBlock(text as TextBlock, language);
			}
			else if (text is TextContainer)
			{
				return representTextContainer(text as TextContainer, language);
			}
			else
			{
				throw new NotSupportedException(nameof(text));
			}
		}

		public StringBuilder representTextBlock(TextBlock text, ILanguage language)
		{
			String result = text.Formatter(language);
			foreach (var parameter in text.Parameters)
			{
				result = result.Replace(parameter.Key, String.Format("\"{0}\"", parameter.Value.Name.GetValue(language)));
			}
			return new StringBuilder(result);
		}

		public StringBuilder representTextContainer(TextContainer text, ILanguage language)
		{
			var result = new StringBuilder();
			foreach (var line in text.Children)
			{
				result.Append(Represent(line, language));
				result.AppendLine();
			}
			return result;
		}
	}
}
