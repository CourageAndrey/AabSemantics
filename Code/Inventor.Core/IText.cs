using System.Collections.Generic;
using System.Text;

namespace Inventor.Core
{
	public interface IText
	{
	}

	public interface ITextContainer : IText
	{
		IList<IText> Children
		{ get; }
	}

	public interface ITextDecorator : IText
	{
		IText InnerText
		{ get; }
	}

#warning Kill this with fire!
	public static class TextExtensions
	{
		public static StringBuilder GetPlainText(this IText text, ILanguage language)
		{
			return ((Text.TextContainer) text).GetPlainText(language);
		}
	}
}
