using System;
using System.Collections.Generic;
using System.Text;

using Inventor.Core.Text.Containers;
using Inventor.Core.Text.Decorators;
using Inventor.Core.Text.Primitives;
using Inventor.Core.Text.Representers;

namespace Inventor.Core
{
	public interface ITextRepresenter
	{
		StringBuilder RepresentText(IText text, ILanguage language);
	}

	public interface IStructuredTextRepresenter : ITextRepresenter
	{
		#region Regular primitives

		StringBuilder Represent(FormattedText text, ILanguage language);

		StringBuilder Represent(LineBreakText text, ILanguage language);

		StringBuilder Represent(SpaceText text, ILanguage language);

		#endregion

		#region Containers

		StringBuilder Represent(BulletsContainer text, ILanguage language);

		StringBuilder Represent(NumberingContainer text, ILanguage language);

		StringBuilder Represent(UnstructuredContainer text, ILanguage language);

		#endregion

		#region Decorators

		StringBuilder Represent(BoldDecorator text, ILanguage language);

		StringBuilder Represent(ItalicDecorator text, ILanguage language);

		StringBuilder Represent(UnderlineDecorator text, ILanguage language);

		StringBuilder Represent(StrikeoutDecorator text, ILanguage language);

		StringBuilder Represent(SubscriptDecorator text, ILanguage language);

		StringBuilder Represent(SuperscriptDecorator text, ILanguage language);

		StringBuilder Represent(HeaderDecorator text, ILanguage language);

		StringBuilder Represent(ParagraphDecorator text, ILanguage language);

		#endregion
	}

	public static class TextRepresenters
	{
		#region List

		public static readonly PlainStringTextRepresenter PlainString = new PlainStringTextRepresenter();

		public static readonly HtmlTextRepresenter Html = new HtmlTextRepresenter();

		public static readonly ICollection<ITextRepresenter> All = new ITextRepresenter[]
		{
			PlainString,
			Html,
		};

		#endregion

		public static StringBuilder Represent(this IStructuredTextRepresenter representer, IText text, ILanguage language)
		{
			#region Regular primitives

			if (text is FormattedText)
			{
				return representer.Represent(text as FormattedText, language);
			}
			else if (text is LineBreakText)
			{
				return representer.Represent(text as LineBreakText, language);
			}
			else if (text is SpaceText)
			{
				return representer.Represent(text as SpaceText, language);
			}

			#endregion

			#region Containers

			else if (text is BulletsContainer)
			{
				return representer.Represent(text as BulletsContainer, language);
			}
			else if (text is NumberingContainer)
			{
				return representer.Represent(text as NumberingContainer, language);
			}
			else if (text is UnstructuredContainer)
			{
				return representer.Represent(text as UnstructuredContainer, language);
			}

			#endregion

			#region Decorators

			if (text is BoldDecorator)
			{
				return representer.Represent(text as BoldDecorator, language);
			}
			else if (text is ItalicDecorator)
			{
				return representer.Represent(text as ItalicDecorator, language);
			}
			else if (text is UnderlineDecorator)
			{
				return representer.Represent(text as UnderlineDecorator, language);
			}
			else if (text is StrikeoutDecorator)
			{
				return representer.Represent(text as StrikeoutDecorator, language);
			}
			else if (text is SubscriptDecorator)
			{
				return representer.Represent(text as SubscriptDecorator, language);
			}
			else if (text is SuperscriptDecorator)
			{
				return representer.Represent(text as SuperscriptDecorator, language);
			}
			else if (text is HeaderDecorator)
			{
				return representer.Represent(text as HeaderDecorator, language);
			}
			else if (text is ParagraphDecorator)
			{
				return representer.Represent(text as ParagraphDecorator, language);
			}

			#endregion

			else
			{
				throw new NotSupportedException(nameof(text));
			}
		}
	}
}
