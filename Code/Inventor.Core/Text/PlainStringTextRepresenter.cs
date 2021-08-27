using System;
using System.Text;

namespace Inventor.Core.Text
{
	public class PlainStringTextRepresenter : IStructuredTextRepresenter
	{
		public StringBuilder RepresentText(IText text, ILanguage language)
		{
			return this.Represent(text, language);
		}

		public StringBuilder Represent(TextBlock textBlock, ILanguage language)
		{
			String result = textBlock.Formatter(language);
			foreach (var parameter in textBlock.Parameters)
			{
				result = result.Replace(parameter.Key, String.Format("\"{0}\"", parameter.Value.Name.GetValue(language)));
			}
			return new StringBuilder(result);
		}

		public StringBuilder Represent(TextContainer textContainer, ILanguage language)
		{
			var result = new StringBuilder();
			foreach (var line in textContainer.Children)
			{
				result.Append(RepresentText(line, language));
				result.AppendLine();
			}
			return result;
		}
	}
}
