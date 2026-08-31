using System;
using System.Collections.Generic;
using System.Text;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;
using AabSemantics.Text.Renders;

namespace AabSemantics
{
	/// <summary>
	/// Turns structured <see cref="IText"/> into a string in some output format.
	/// </summary>
	public interface ITextRender
	{
		/// <summary>
		/// Renders a text tree.
		/// </summary>
		/// <param name="text">Text to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder RenderText(IText text, ILanguage language);
	}

	/// <summary>
	/// A render that handles each kind of text node separately. Implementations provide one
	/// overload per node type; dispatching between them is done by
	/// <see cref="TextRenders.Render(IStructuredTextRender, IText, ILanguage)"/>.
	/// </summary>
	public interface IStructuredTextRender : ITextRender
	{
		#region Regular primitives

		/// <summary>
		/// Renders a localizable sentence with its parameters substituted.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(FormattedText text, ILanguage language);

		/// <summary>
		/// Renders a line break.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(LineBreakText text, ILanguage language);

		/// <summary>
		/// Renders a space.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(SpaceText text, ILanguage language);

		#endregion

		#region Containers

		/// <summary>
		/// Renders an unordered list.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(BulletsContainer text, ILanguage language);

		/// <summary>
		/// Renders a numbered list.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(NumberingContainer text, ILanguage language);

		/// <summary>
		/// Renders a plain sequence of nested texts, without any list markup.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(UnstructuredContainer text, ILanguage language);

		#endregion

		#region Decorators

		/// <summary>
		/// Renders bold text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(BoldDecorator text, ILanguage language);

		/// <summary>
		/// Renders italic text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(ItalicDecorator text, ILanguage language);

		/// <summary>
		/// Renders underlined text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(UnderlineDecorator text, ILanguage language);

		/// <summary>
		/// Renders struck-out text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(StrikeoutDecorator text, ILanguage language);

		/// <summary>
		/// Renders subscript text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(SubscriptDecorator text, ILanguage language);

		/// <summary>
		/// Renders superscript text.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(SuperscriptDecorator text, ILanguage language);

		/// <summary>
		/// Renders a heading.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(HeaderDecorator text, ILanguage language);

		/// <summary>
		/// Renders a paragraph.
		/// </summary>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		StringBuilder Render(ParagraphDecorator text, ILanguage language);

		#endregion
	}

	/// <summary>
	/// The renders shipped with the engine, plus the dispatcher that routes a text node to the
	/// matching <see cref="IStructuredTextRender"/> overload.
	/// </summary>
	public static class TextRenders
	{
		#region List

		/// <summary>
		/// Renders text as plain, unformatted string.
		/// </summary>
		public static readonly PlainStringTextRender PlainString = new PlainStringTextRender();

		/// <summary>
		/// Renders text as an HTML fragment.
		/// </summary>
		public static readonly HtmlTextRender Html = new HtmlTextRender();

		/// <summary>
		/// Renders text as Markdown.
		/// </summary>
		public static readonly MakefileTextRender Markdown = new MakefileTextRender();

		/// <summary>
		/// Every built-in render.
		/// </summary>
		public static readonly ICollection<ITextRender> All = new ITextRender[]
		{
			PlainString,
			Html,
			Markdown,
		};

		#endregion

		/// <summary>
		/// Dispatches a text node to the render overload matching its runtime type.
		/// Adding a new node type requires extending both <see cref="IStructuredTextRender"/>
		/// and this method.
		/// </summary>
		/// <param name="render">Render to dispatch to.</param>
		/// <param name="text">Node to render.</param>
		/// <param name="language">Language to resolve localizable strings in.</param>
		/// <returns>The rendered output.</returns>
		/// <exception cref="NotSupportedException">The node type is not one of the known ones.</exception>
		public static StringBuilder Render(this IStructuredTextRender render, IText text, ILanguage language)
		{
			#region Regular primitives

			if (text is FormattedText)
			{
				return render.Render(text as FormattedText, language);
			}
			else if (text is LineBreakText)
			{
				return render.Render(text as LineBreakText, language);
			}
			else if (text is SpaceText)
			{
				return render.Render(text as SpaceText, language);
			}

			#endregion

			#region Containers

			else if (text is BulletsContainer)
			{
				return render.Render(text as BulletsContainer, language);
			}
			else if (text is NumberingContainer)
			{
				return render.Render(text as NumberingContainer, language);
			}
			else if (text is UnstructuredContainer)
			{
				return render.Render(text as UnstructuredContainer, language);
			}

			#endregion

			#region Decorators

			if (text is BoldDecorator)
			{
				return render.Render(text as BoldDecorator, language);
			}
			else if (text is ItalicDecorator)
			{
				return render.Render(text as ItalicDecorator, language);
			}
			else if (text is UnderlineDecorator)
			{
				return render.Render(text as UnderlineDecorator, language);
			}
			else if (text is StrikeoutDecorator)
			{
				return render.Render(text as StrikeoutDecorator, language);
			}
			else if (text is SubscriptDecorator)
			{
				return render.Render(text as SubscriptDecorator, language);
			}
			else if (text is SuperscriptDecorator)
			{
				return render.Render(text as SuperscriptDecorator, language);
			}
			else if (text is HeaderDecorator)
			{
				return render.Render(text as HeaderDecorator, language);
			}
			else if (text is ParagraphDecorator)
			{
				return render.Render(text as ParagraphDecorator, language);
			}

			#endregion

			else
			{
				throw new NotSupportedException(nameof(text));
			}
		}
	}
}
