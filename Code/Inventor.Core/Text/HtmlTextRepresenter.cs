using System;
using System.Text;
using System.Web;

namespace Inventor.Core.Text
{
	public class HtmlTextRepresenter : ITextRepresenter<StringBuilder>
	{
		public StringBuilder Represent(IText text, ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.Append(RepresentWithoutWrapping(text, language));
			result.Append(@"</body></html>");
			return result;
		}

		private StringBuilder RepresentWithoutWrapping(IText text, ILanguage language)
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
				result = result.Replace(
					parameter.Key,
					String.Format("<a href=\"{0}\">{1}</a>", parameter.Value.ID, HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))));
			}
			return new StringBuilder(result + @"<br/><br/>");
		}

		public StringBuilder representTextContainer(TextContainer text, ILanguage language)
		{
			var result = new StringBuilder();
			foreach (var line in text.Children)
			{
				result.Append(RepresentWithoutWrapping(line, language));
			}
			return result;
		}
	}
}
