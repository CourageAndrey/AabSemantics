using System;
using System.Text;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Text.Renders
{
	public class PlainStringTextRender : IStructuredTextRender
	{
		public virtual StringBuilder RenderText(IText text, ILanguage language)
		{
			return this.Render(text, language);
		}

		#region Regular primitives

		public virtual StringBuilder Render(FormattedText text, ILanguage language)
		{
			String result = text.Formatter(language);
			foreach (var parameter in text.Parameters)
			{
				result = result.Replace(parameter.Key, $"\"{parameter.Value.Name.GetValue(language)}\"");
			}
			return new StringBuilder(result);
		}

		public virtual StringBuilder Render(LineBreakText text, ILanguage language)
		{
			var result = new StringBuilder();
			result.AppendLine();
			return result;
		}

		public virtual StringBuilder Render(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" ");
		}

		#endregion

		#region Containers

		public virtual StringBuilder Render(BulletsContainer text, ILanguage language)
		{
			return renderContainer(
				text,
				language,
				(lineNumber, lineCount) => " * ");
		}

		public virtual StringBuilder Render(NumberingContainer text, ILanguage language)
		{
			return renderContainer(
				text,
				language,
				(lineNumber, lineCount) => $" {lineNumber.ToString().PadLeft(lineCount.ToString().Length, ' ')}. ");
		}

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

		public virtual StringBuilder Render(BoldDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "*");
		}

		public virtual StringBuilder Render(ItalicDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "@");
		}

		public virtual StringBuilder Render(UnderlineDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "_");
		}

		public virtual StringBuilder Render(StrikeoutDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "~");
		}

		public virtual StringBuilder Render(SubscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "˅");
		}

		public virtual StringBuilder Render(SuperscriptDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, "^");
		}

		public virtual StringBuilder Render(HeaderDecorator text, ILanguage language)
		{
			return renderDecorator(text, language, new String('#', text.Level));
		}

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
