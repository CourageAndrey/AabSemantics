using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;

namespace AabSemantics
{
	/// <summary>
	/// A piece of structured, language-independent text. Text is built as a tree of primitives,
	/// containers and decorators, and only turned into a string by an <see cref="ITextRender"/> —
	/// which is what lets the same answer be rendered as plain text or as HTML.
	/// </summary>
	public interface IText
	{
		/// <summary>
		/// Collects the knowledge items referenced from this text and everything nested in it,
		/// keyed by the anchor token standing for each of them. Renders use this to turn
		/// anchors into links.
		/// </summary>
		/// <returns>Anchor token to referenced item.</returns>
		IDictionary<String, IKnowledge> GetParameters();
	}

	/// <summary>
	/// Text composed of an ordered sequence of nested texts, such as a paragraph or a bullet list.
	/// </summary>
	public interface ITextContainer : IText
	{
		/// <summary>
		/// Nested texts, in rendering order. The list is mutable; see <see cref="TextExtensions"/>
		/// for fluent ways to extend it.
		/// </summary>
		IList<IText> Items
		{ get; }
	}

	/// <summary>
	/// Text that wraps exactly one other text to change how it is presented, such as bold or a header.
	/// </summary>
	public interface ITextDecorator : IText
	{
		/// <summary>
		/// The wrapped text.
		/// </summary>
		IText InnerText
		{ get; }
	}

	/// <summary>
	/// Fluent construction of structured text: concatenation and decoration.
	/// </summary>
	public static class TextExtensions
	{
		#region Concatenation methods

		/// <summary>
		/// Concatenates two texts.
		/// </summary>
		/// <param name="text1">
		/// First text. When it already is an unstructured container it is extended in place;
		/// otherwise a new container is created around it.
		/// </param>
		/// <param name="text2">Text to append.</param>
		/// <returns>The container holding both texts.</returns>
		public static ITextContainer Append(this IText text1, IText text2)
		{
			var container = text1 as UnstructuredContainer;
			if (container == null)
			{
				container = new UnstructuredContainer(text1);
			}
			return container.Append(text2);
		}

		/// <summary>
		/// Appends a text to a container.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <param name="line">Text to append.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer Append(this ITextContainer textContainer, IText line)
		{
			textContainer.Items.Add(line);
			return textContainer;
		}

		/// <summary>
		/// Appends a localizable sentence to a container.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <param name="formatter">Selects the format string from a language; evaluated at render time.</param>
		/// <param name="parameters">Knowledge items the format string refers to by anchor.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer Append(this ITextContainer textContainer, Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters = null)
		{
			textContainer.Items.Add(new FormattedText(formatter, parameters));
			return textContainer;
		}

		/// <summary>
		/// Appends a line break.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer AppendLineBreak(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new LineBreakText());
			return textContainer;
		}

		/// <summary>
		/// Appends a single space.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer AppendSpace(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new SpaceText());
			return textContainer;
		}

		/// <summary>
		/// Appends an unordered list.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <param name="items">List entries; materialized immediately.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer AppendBulletsList(this ITextContainer textContainer, IEnumerable<IText> items)
		{
			textContainer.Items.Add(new BulletsContainer(items.ToList()));
			return textContainer;
		}

		/// <summary>
		/// Appends a numbered list.
		/// </summary>
		/// <param name="textContainer">Container to extend in place.</param>
		/// <param name="items">List entries; materialized immediately.</param>
		/// <returns>The same container, to allow call chaining.</returns>
		public static ITextContainer AppendNumberingList(this ITextContainer textContainer, IEnumerable<IText> items)
		{
			textContainer.Items.Add(new NumberingContainer(items.ToList()));
			return textContainer;
		}

		#endregion

		#region Decorations

		/// <summary>
		/// Wraps the text in bold formatting.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static BoldDecorator MakeBold(this IText text)
		{
			return new BoldDecorator(text);
		}

		/// <summary>
		/// Wraps the text in italic formatting.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static ItalicDecorator MakeItalic(this IText text)
		{
			return new ItalicDecorator(text);
		}

		/// <summary>
		/// Wraps the text in underline formatting.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static UnderlineDecorator MakeUnderline(this IText text)
		{
			return new UnderlineDecorator(text);
		}

		/// <summary>
		/// Wraps the text in strikeout formatting.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static StrikeoutDecorator MakeStrikeout(this IText text)
		{
			return new StrikeoutDecorator(text);
		}

		/// <summary>
		/// Wraps the text as subscript.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static SubscriptDecorator MakeSubscript(this IText text)
		{
			return new SubscriptDecorator(text);
		}

		/// <summary>
		/// Wraps the text as superscript.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static SuperscriptDecorator MakeSuperscript(this IText text)
		{
			return new SuperscriptDecorator(text);
		}

		/// <summary>
		/// Wraps the text as a heading.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <param name="level">Heading level, as in HTML's <c>h1</c>..<c>h6</c>.</param>
		/// <returns>The decorated text.</returns>
		public static HeaderDecorator MakeHeader(this IText text, Byte level)
		{
			return new HeaderDecorator(text, level);
		}

		/// <summary>
		/// Wraps the text in a paragraph.
		/// </summary>
		/// <param name="text">Text to decorate.</param>
		/// <returns>The decorated text.</returns>
		public static ParagraphDecorator MakeParagraph(this IText text)
		{
			return new ParagraphDecorator(text);
		}

		#endregion
	}
}
