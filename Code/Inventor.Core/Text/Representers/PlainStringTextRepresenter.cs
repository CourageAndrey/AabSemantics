using System;
using System.Text;

using Inventor.Core.Text.Containers;
using Inventor.Core.Text.Decorators;
using Inventor.Core.Text.Primitives;

namespace Inventor.Core.Text.Representers
{
	public class PlainStringTextRepresenter : IStructuredTextRepresenter
	{
		public virtual StringBuilder RepresentText(IText text, ILanguage language)
		{
			return this.Represent(text, language);
		}

		#region Regular primitives

		public virtual StringBuilder Represent(FormattedText text, ILanguage language)
		{
			String result = text.Formatter(language);
			foreach (var parameter in text.Parameters)
			{
				result = result.Replace(parameter.Key, $"\"{parameter.Value.Name.GetValue(language)}\"");
			}
			return new StringBuilder(result);
		}

		public virtual StringBuilder Represent(LineBreakText text, ILanguage language)
		{
			var result = new StringBuilder();
			result.AppendLine();
			return result;
		}

		public virtual StringBuilder Represent(SpaceText text, ILanguage language)
		{
			return new StringBuilder(" ");
		}

		#endregion

		#region Containers

		public virtual StringBuilder Represent(BulletsContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				(lineNumber, lineCount) => " * ");
		}

		public virtual StringBuilder Represent(NumberingContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				(lineNumber, lineCount) => $" {lineNumber.ToString().PadLeft(lineCount.ToString().Length, ' ')}. ");
		}

		public virtual StringBuilder Represent(UnstructuredContainer text, ILanguage language)
		{
			return representContainer(
				text,
				language,
				(lineNumber, lineCount) => String.Empty);
		}

		private StringBuilder representContainer(
			ITextContainer container,
			ILanguage language,
			Func<Int32, Int32, String> getLineIndent)
		{
			var result = new StringBuilder();
			result.AppendLine();
			for (var i = 0; i < container.Items.Count; i++)
			{
				result.Append(getLineIndent(i + 1, container.Items.Count));
				result.Append(RepresentText(container.Items[i], language));
				result.AppendLine();
			}
			return result;
		}

		#endregion

		#region Decorators

		public virtual StringBuilder Represent(BoldDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "*");
		}

		public virtual StringBuilder Represent(ItalicDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "@");
		}

		public virtual StringBuilder Represent(UnderlineDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "_");
		}

		public virtual StringBuilder Represent(StrikeoutDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "~");
		}

		public virtual StringBuilder Represent(SubscriptDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "˅");
		}

		public virtual StringBuilder Represent(SuperscriptDecorator text, ILanguage language)
		{
			return representDecorator(text, language, "^");
		}

		public virtual StringBuilder Represent(HeaderDecorator text, ILanguage language)
		{
			return representDecorator(text, language, new String('#', text.Level));
		}

		public virtual StringBuilder Represent(ParagraphDecorator text, ILanguage language)
		{
			var result = new StringBuilder();
			result.AppendLine();
			result.Append("\t");
			result.Append(this.Represent(text.InnerText, language));
			result.AppendLine();
			return result;
		}

		private StringBuilder representDecorator(
			ITextDecorator decorator,
			ILanguage language,
			String wrappingSymbol)
		{
			var result = new StringBuilder(wrappingSymbol);
			result.Append(this.Represent(decorator.InnerText, language));
			result.Append(wrappingSymbol);
			return result;
		}

		#endregion
	}
}
