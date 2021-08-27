using System.Text;

namespace Inventor.Core
{
	public interface IText
	{
	}

#warning Kill this with fire!
	public static class TextExtensions
	{
		public static StringBuilder GetPlainText(this IText text, ILanguage language)
		{
			return ((Text.FormattedText) text).GetPlainText(language);
		}
	}
}
