using System;
using System.Text;
using System.Web;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Text.Renders
{
	public class HtmlTextRender : IStructuredTextRender
	{
		public virtual StringBuilder RenderText(IText text, ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.Append(this.Render(text, language));
			result.Append(@"</body></html>");
			return result;
		}

		#region Regular primitives

		public virtual StringBuilder Render(FormattedText text, ILanguage language)
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

		public virtual StringBuilder Render(LineBreakText text, ILanguage language)
		{
			return new StringBuilder("<br/><br/>");
		}

		public virtual StringBuilder Render(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" &nbsp; ");
		}

		#endregion

		#region Containers

		public virtual StringBuilder Render(BulletsContainer text, ILanguage language)
		{
			return renderContainer(
				text,
				language,
				"<ul>",
				"</ul>",
				"<li>",
				"</li>");
		}

		public virtual StringBuilder Render(NumberingContainer text, ILanguage language)
		{
			return renderContainer(
				text,
				language,
				"<ol>",
				"</ol>",
				"<li>",
				"</li>");
		}

		public virtual StringBuilder Render(UnstructuredContainer text, ILanguage language)
		{
			return renderContainer(
				text,
				language,
				String.Empty,
				String.Empty,
				String.Empty,
				String.Empty);
		}

		private StringBuilder renderContainer(
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
				result.Append(this.Render(item, language));
				result.Append(endItemTag);
			}
			result.AppendLine(afterTag);
			return result;
		}

		#endregion

		#region Decorators

		public virtual StringBuilder Render(BoldDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<b>", "</b>");
		}

		public virtual StringBuilder Render(ItalicDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<i>", "</i>");
		}

		public virtual StringBuilder Render(UnderlineDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<u>", "</u>");
		}

		public virtual StringBuilder Render(StrikeoutDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<s>", "</s>");
		}

		public virtual StringBuilder Render(SubscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<sub>", "</sub>");
		}

		public virtual StringBuilder Render(SuperscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<sup>", "</sup>");
		}

		public virtual StringBuilder Render(HeaderDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, $"<h{text.Level}>", $"</h{text.Level}>");
		}

		public virtual StringBuilder Render(ParagraphDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<p>", "</p>");
		}

		private StringBuilder renderDecorator(
			ITextDecorator decorator,
			ILanguage language,
			String beforeTag,
			String afterTag)
		{
			var result = new StringBuilder(beforeTag);
			result.Append(this.Render(decorator.InnerText, language));
			result.Append(afterTag);
			return result;
		}

		#endregion
	}
}
