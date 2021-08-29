using System;

namespace Inventor.Core
{
	public interface IKnowledge : INamed, IIdentifiable
	{
		ILocalizedString Hint
		{ get; }
	}

	public static class KnowledgeHelper
	{
		public static String GetAnchor(this IKnowledge knowledge)
		{
			return $"#{knowledge.ID}#";
		}
	}
}
