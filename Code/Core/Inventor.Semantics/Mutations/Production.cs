using System;

namespace Inventor.Semantics.Mutations
{
	public class Production : IMutation
	{
		public IsomorphicSearchPattern LookupPattern
		{ get; }

		public Action<KnowledgeStructure> ApplyMethod
		{ get; }

		public Production(IsomorphicSearchPattern lookupPattern, Action<KnowledgeStructure> applyMethod)
		{
			if (lookupPattern != null)
			{
				LookupPattern = lookupPattern;
			}
			else
			{
				throw new ArgumentNullException(nameof(lookupPattern));
			}

			if (applyMethod != null)
			{
				ApplyMethod = applyMethod;
			}
			else
			{
				throw new ArgumentNullException(nameof(applyMethod));
			}
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
