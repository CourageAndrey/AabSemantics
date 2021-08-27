using System;
using System.Text;
using System.Web;

namespace Inventor.Core.Text
{
	public class HtmlTextRepresenter : IStructuredTextRepresenter
	{
		public StringBuilder RepresentText(IText text, ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.Append(this.Represent(text, language));
			result.Append(@"</body></html>");
			return result;
		}

		public StringBuilder Represent(TextBlock textBlock, ILanguage language)
		{
			String result = textBlock.Formatter(language);
			foreach (var parameter in textBlock.Parameters)
			{
				result = result.Replace(
					parameter.Key,
					String.Format("<a href=\"{0}\">{1}</a>", parameter.Value.ID, HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))));
			}
			return new StringBuilder(result + @"<br/><br/>");
		}

		public StringBuilder Represent(TextContainer textContainer, ILanguage language)
		{
			var result = new StringBuilder();
			foreach (var line in textContainer.Children)
			{
				result.Append(this.Represent(line, language));
			}
			return result;
		}
	}
}
