using System;
using System.Text;
using System.Web;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Text.Renders
{
	/// <summary>Renders text as an HTML fragment, mapping each node kind to the matching tags.</summary>
	public class HtmlTextRender : IStructuredTextRender
	{
		/// <summary>Renders a text tree as HTML.</summary>
		/// <param name="text">Text to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder RenderText(IText text, ILanguage language)
		{
			var result = new StringBuilder(@"<html><head><title>Inventor</title></head><body>");
			result.Append(this.Render(text, language));
			result.Append(@"</body></html>");
			return result;
		}

		#region Regular primitives

		/// <summary>Renders a localizable sentence, substituting its parameters.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
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

		/// <summary>Renders a line break.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(LineBreakText text, ILanguage language)
		{
			return new StringBuilder("<br/><br/>");
		}

		/// <summary>Renders a space.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" &nbsp; ");
		}

		#endregion

		#region Containers

		/// <summary>Renders an unordered list.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
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

		/// <summary>Renders a numbered list.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
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

		/// <summary>Renders a plain sequence of nested texts.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
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

		/// <summary>Renders bold text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(BoldDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<b>", "</b>");
		}

		/// <summary>Renders italic text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(ItalicDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<i>", "</i>");
		}

		/// <summary>Renders underlined text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(UnderlineDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<u>", "</u>");
		}

		/// <summary>Renders struck-out text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(StrikeoutDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<s>", "</s>");
		}

		/// <summary>Renders subscript text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SubscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<sub>", "</sub>");
		}

		/// <summary>Renders superscript text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SuperscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "<sup>", "</sup>");
		}

		/// <summary>Renders a heading.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(HeaderDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, $"<h{text.Level}>", $"</h{text.Level}>");
		}

		/// <summary>Renders a paragraph.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
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
