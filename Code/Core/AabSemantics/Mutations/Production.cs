using System;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	public class Production : IMutation
	{
		public IsomorphicSearchPattern LookupPattern
		{ get; }

		public Action<KnowledgeStructure> ApplyMethod
		{ get; }

		public Production(IsomorphicSearchPattern lookupPattern, Action<KnowledgeStructure> applyMethod)
		{
			LookupPattern = lookupPattern.EnsureNotNull(nameof(lookupPattern));
			ApplyMethod = applyMethod.EnsureNotNull(nameof(applyMethod));
		}

		public Boolean TryToApply(ISemanticNetwork semanticNetwork)
		{
			var match = semanticNetwork.FindFirstMatch(LookupPattern);
			if (match != null)
			{
				ApplyMethod(match);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
