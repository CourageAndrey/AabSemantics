using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IText
	{
		IDictionary<String, IKnowledge> GetParameters();
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
