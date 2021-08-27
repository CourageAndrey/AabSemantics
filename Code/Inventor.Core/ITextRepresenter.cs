using System;
using System.Text;

using Inventor.Core.Text;

namespace Inventor.Core
{
	public interface ITextRepresenter
	{
		StringBuilder RepresentText(IText text, ILanguage language);
	}

	public interface IStructuredTextRepresenter : ITextRepresenter
	{
		StringBuilder Represent(TextBlock textBlock, ILanguage language);

		StringBuilder Represent(TextContainer textContainer, ILanguage language);
	}

	public static class TextRepresenters
	{
		#region List

		public static readonly PlainStringTextRepresenter PlainString = new PlainStringTextRepresenter();

		public static readonly HtmlTextRepresenter Html = new HtmlTextRepresenter();

		#endregion

		public static StringBuilder Represent(this IStructuredTextRepresenter representer, IText text, ILanguage language)
		{
			if (text is TextBlock)
			{
				return representer.Represent(text as TextBlock, language);
			}
			else if (text is TextContainer)
			{
				return representer.Represent(text as TextContainer, language);
			}
			else
			{
				throw new NotSupportedException(nameof(text));
			}
		}
	}
}
