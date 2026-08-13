using System;
using System.Text;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Text.Renders
{
	/// <summary>
	/// Renders text as a plain string, without markup. Lists are indented with plain characters,
	/// and decorators such as bold pass their content through unchanged.
	/// </summary>
	public class PlainStringTextRender : IStructuredTextRender
	{
		/// <summary>Renders a text tree as plain text.</summary>
		/// <param name="text">Text to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder RenderText(IText text, ILanguage language)
		{
			return this.Render(text, language);
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
				result = result.Replace(parameter.Key, $"\"{parameter.Value.Name.GetValue(language)}\"");
			}
			return new StringBuilder(result);
		}

		/// <summary>Renders a line break.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(LineBreakText text, ILanguage language)
		{
			var result = new StringBuilder();
			result.AppendLine();
			return result;
		}

		/// <summary>Renders a space.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" ");
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
				(lineNumber, lineCount) => " * ");
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
				(lineNumber, lineCount) => $" {lineNumber.ToString().PadLeft(lineCount.ToString().Length, ' ')}. ");
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
				(lineNumber, lineCount) => String.Empty);
		}

		private StringBuilder renderContainer(
			ITextContainer container,
			ILanguage language,
			Func<Int32, Int32, String> getLineIndent)
		{
			var result = new StringBuilder();
			result.AppendLine();
			for (var i = 0; i < container.Items.Count; i++)
			{
				result.Append(getLineIndent(i + 1, container.Items.Count));
				result.Append(RenderText(container.Items[i], language));
				result.AppendLine();
			}
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
			return renderDecorator(text, language, "*");
		}

		/// <summary>Renders italic text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(ItalicDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "@");
		}

		/// <summary>Renders underlined text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(UnderlineDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "_");
		}

		/// <summary>Renders struck-out text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(StrikeoutDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "~");
		}

		/// <summary>Renders subscript text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SubscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "˅");
		}

		/// <summary>Renders superscript text.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(SuperscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "^");
		}

		/// <summary>Renders a heading.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(HeaderDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, new String('#', text.Level));
		}

		/// <summary>Renders a paragraph.</summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		public virtual StringBuilder Render(ParagraphDecorator text, ILanguage language)
		{
			var result = new StringBuilder();
			result.AppendLine();
			result.Append("\t");
			result.Append(this.Render(text.InnerText, language));
			result.AppendLine();
			return result;
		}

		private StringBuilder renderDecorator(
			ITextDecorator decorator,
			ILanguage language,
			String wrappingSymbol)
		{
			var result = new StringBuilder(wrappingSymbol);
			result.Append(this.Render(decorator.InnerText, language));
			result.Append(wrappingSymbol);
			return result;
		}

		#endregion
	}
}
