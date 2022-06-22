using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Mutations
{
	public abstract class IsomorphicSearchPattern
	{
		public abstract IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork);
	}

	public static class IsomorphicSearchPatternExtensions
	{
		public static Boolean DoesMatch(this ISemanticNetwork semanticNetwork, IsomorphicSearchPattern isomorphicSearchPattern)
		{
			return isomorphicSearchPattern.FindMatches(semanticNetwork).Any();
		}

		public static KnowledgeStructure FindFirstMatch(this ISemanticNetwork semanticNetwork, IsomorphicSearchPattern isomorphicSearchPattern)
		{
			return isomorphicSearchPattern.FindMatches(semanticNetwork).FirstOrDefault();
		}
	}
}
