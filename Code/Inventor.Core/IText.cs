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
		public static void Append(this ITextContainer textContainer, IText line)
		{
			textContainer.Items.Add(line);
		}

		public static void Append(this ITextContainer textContainer, Func<ILanguage, String> formatter, IDictionary<String, IKnowledge> parameters)
		{
			textContainer.Items.Add(new FormattedText(formatter, parameters));
		}

		public static void AppendLineBreak(this ITextContainer textContainer)
		{
			textContainer.Items.Add(new LineBreakText());
		}

#warning Add other extensions.
	}
}
