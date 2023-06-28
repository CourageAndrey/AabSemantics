using System;
using System.Collections.Generic;
using System.Text;

using AabSemantics.Text.Containers;
using AabSemantics.Text.Decorators;
using AabSemantics.Text.Primitives;
using AabSemantics.Text.Renders;

namespace AabSemantics
{
	public interface ITextRender
	{
		StringBuilder RenderText(IText text, ILanguage language);
	}

	public interface IStructuredTextRender : ITextRender
	{
		#region Regular primitives

		StringBuilder Render(FormattedText text, ILanguage language);

		StringBuilder Render(LineBreakText text, ILanguage language);

		StringBuilder Render(SpaceText text, ILanguage language);

		#endregion

		#region Containers

		StringBuilder Render(BulletsContainer text, ILanguage language);

		StringBuilder Render(NumberingContainer text, ILanguage language);

		StringBuilder Render(UnstructuredContainer text, ILanguage language);

		#endregion

		#region Decorators

		StringBuilder Render(BoldDecorator text, ILanguage language);

		StringBuilder Render(ItalicDecorator text, ILanguage language);

		StringBuilder Render(UnderlineDecorator text, ILanguage language);

		StringBuilder Render(StrikeoutDecorator text, ILanguage language);

		StringBuilder Render(SubscriptDecorator text, ILanguage language);

		StringBuilder Render(SuperscriptDecorator text, ILanguage language);

		StringBuilder Render(HeaderDecorator text, ILanguage language);

		StringBuilder Render(ParagraphDecorator text, ILanguage language);

		#endregion
	}

	public static class TextRenders
	{
		#region List

		public static readonly PlainStringTextRender PlainString = new PlainStringTextRender();

		public static readonly HtmlTextRender Html = new HtmlTextRender();

		public static readonly ICollection<ITextRender> All = new ITextRender[]
		{
			PlainString,
			Html,
		};

		#endregion

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
