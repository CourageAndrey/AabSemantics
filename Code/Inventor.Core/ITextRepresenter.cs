using Inventor.Core.Text;

namespace Inventor.Core
{
	public interface ITextRepresenter<out TResult>
	{
		TResult Represent(IText text, ILanguage language);
	}

	public static class TextRepresenters
	{
		public static readonly PlainStringTextRepresenter PlainString = new PlainStringTextRepresenter();

		public static readonly HtmlTextRepresenter Html = new HtmlTextRepresenter();
	}
}
