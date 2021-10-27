using System;
using System.Text;
using System.Web;

using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Text.Decorators;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Text.Representers
{
	public class HtmlTextRepresenter : IStructuredTextRepresenter
	{
		public virtual StringBuilder RepresentText(IText text, ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.Append(this.Represent(text, language));
			result.Append(@"</body></html>");
			return result;
		}

		#region Regular primitives

		public virtual StringBuilder Represent(FormattedText text, ILanguage language)
		{
			String result = text.Formatter(language);
			foreach (var parameter in text.Parameters)
			{
				result = result.Replace(
					parameter.Key,
					$"<a href=\"{parameter.Value.ID}\">{HttpUtility.HtmlEncode(parameter.Value.Name.GetValue(language))}</a>");
			}
			return new StringBuilder(result + "<br/>");
		}

		public virtual StringBuilder Represent(LineBreakText text, ILanguage language)
		{
			return new StringBuilder("<br/><br/>");
		}

		public virtual StringBuilder Represent(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" &nbsp; ");
		}

		#endregion

		#region Containers

		public virtual StringBuilder Represent(BulletsContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				"<ul>",
				"</ul>",
				"<li>",
				"</li>");
		}

		public virtual StringBuilder Represent(NumberingContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				"<ol>",
				"</ol>",
				"<li>",
				"</li>");
		}

		public virtual StringBuilder Represent(UnstructuredContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				String.Empty,
				String.Empty,
				String.Empty,
				String.Empty);
		}

		private StringBuilder representContainer(
			ITextContainer container,
			ILanguage language,
			String beforeTag,
			String afterTag,
			String beginItemTag,
			String endItemTag)
		{
			var result = new StringBuilder(beforeTag);
			foreach (var item in container.Items)
			{
				result.Append(beginItemTag);
				result.Append(this.Represent(item, language));
				result.Append(endItemTag);
			}
			result.AppendLine(afterTag);
			return result;
		}

		#endregion

		#region Decorators

		public virtual StringBuilder Represent(BoldDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<b>", "</b>");
		}

		public virtual StringBuilder Represent(ItalicDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<i>", "</i>");
		}

		public virtual StringBuilder Represent(UnderlineDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<u>", "</u>");
		}

		public virtual StringBuilder Represent(StrikeoutDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<s>", "</s>");
		}

		public virtual StringBuilder Represent(SubscriptDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<sub>", "</sub>");
		}

		public virtual StringBuilder Represent(SuperscriptDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<sup>", "</sup>");
		}

		public virtual StringBuilder Represent(HeaderDecorator text, ILanguage language)
		{
			return representDecorator(text, language, $"<h{text.Level}>", $"</h{text.Level}>");
		}

		public virtual StringBuilder Represent(ParagraphDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "<p>", "</p>");
		}

		private StringBuilder representDecorator(
			ITextDecorator decorator,
			ILanguage language,
			String beforeTag,
			String afterTag)
		{
			var result = new StringBuilder(beforeTag);
			result.Append(this.Represent(decorator.InnerText, language));
			result.Append(afterTag);
			return result;
		}

		#endregion
	}
}
