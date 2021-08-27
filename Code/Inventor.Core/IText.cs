using System.Collections.Generic;

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
}
