using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Text.Decorators;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics
{
	public interface IText
	{
		IDictionary<String, IKnowledge> GetParameters();
	}

	public interface ITextContainer : IText
	{
		IList<IText> Items
		{ get; }
	}

	public interface ITextDecorator : IText
	{
		IText InnerText
		{ get; }
	}

	public static class TextExtensions
	{
		#region Concatenation methods

		public static ITextContainer Append(this IText text1, IText text2)
		{
			var container = text1 as UnstructuredContainer;
			if (container == null)
			{
				container = new UnstructuredContainer(text1);
			}
			return container.Append(text2);
		}

		public static ITextContainer Append(this ITextContainer textContainer, IText line)
		{
			textContainer.Items.Add(line);
			return textContainer;
		}

		public static ITextContainer Append(this ITextContainer textContainer, Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters = null)
		{
			textContainer.Items.Add(new FormattedText(formatter, parameters));
			return textContainer;
		}

		public static ITextContainer AppendLineBreak(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new LineBreakText());
			return textContainer;
		}

		public static ITextContainer AppendSpace(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new SpaceText());
			return textContainer;
		}

		public static ITextContainer AppendBulletsList(this ITextContainer textContainer, IEnumerable<IText> items)
		{
			textContainer.Items.Add(new BulletsContainer(items.ToList()));
			return textContainer;
		}

		public static ITextContainer AppendNumberingList(this ITextContainer textContainer, IEnumerable<IText> items)
		{
			textContainer.Items.Add(new NumberingContainer(items.ToList()));
			return textContainer;
		}

		#endregion

		#region Decorations

		public static BoldDecorator MakeBold(this IText text)
		{
			return new BoldDecorator(text);
		}

		public static ItalicDecorator MakeItalic(this IText text)
		{
			return new ItalicDecorator(text);
		}

		public static UnderlineDecorator MakeUnderline(this IText text)
		{
			return new UnderlineDecorator(text);
		}

		public static StrikeoutDecorator MakeStrikeout(this IText text)
		{
			return new StrikeoutDecorator(text);
		}

		public static SubscriptDecorator MakeSubscript(this IText text)
		{
			return new SubscriptDecorator(text);
		}

		public static SuperscriptDecorator MakeSuperscript(this IText text)
		{
			return new SuperscriptDecorator(text);
		}

		public static HeaderDecorator MakeHeader(this IText text, Byte level)
		{
			return new HeaderDecorator(text, level);
		}

		public static ParagraphDecorator MakeParagraph(this IText text)
		{
			return new ParagraphDecorator(text);
		}

		#endregion
	}
}
