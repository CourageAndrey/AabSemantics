using System;
using System.Collections.Generic;

using Inventor.Core.Text;

namespace Inventor.Core
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
		public static ITextContainer Append(this ITextContainer textContainer, IText line)
		{
			textContainer.Items.Add(line);
			return textContainer;
		}

		public static ITextContainer Append(this ITextContainer textContainer, Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			textContainer.Items.Add(new FormattedText(formatter, parameters));
			return textContainer;
		}

		public static ITextContainer AppendLineBreak(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new LineBreakText());
			return textContainer;
		}

#warning Add other extensions.
	}
}
