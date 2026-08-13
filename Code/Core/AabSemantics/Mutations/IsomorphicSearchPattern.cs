using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Mutations
{
	/// <summary>
	/// A shape to look for in a semantic network. Patterns compose: a statement pattern may
	/// constrain the concepts its statements relate, and each part of the composition becomes a
	/// separately addressable binding in the resulting <see cref="KnowledgeStructure"/>.
	/// </summary>
	public abstract class IsomorphicSearchPattern
	{
		/// <summary>Finds every occurrence of the pattern in a network.</summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <returns>Lazily evaluated matches; empty when the pattern does not occur.</returns>
		public abstract IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork);
	}

	/// <summary>Shorthands for the two common questions asked of a pattern.</summary>
	public static class IsomorphicSearchPatternExtensions
	{
		/// <summary>Determines whether the pattern occurs at all, stopping at the first match.</summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <param name="isomorphicSearchPattern">Pattern to look for.</param>
		/// <returns><c>true</c> if the pattern occurs.</returns>
		public static Boolean DoesMatch(this ISemanticNetwork semanticNetwork, IsomorphicSearchPattern isomorphicSearchPattern)
		{
			return isomorphicSearchPattern.FindMatches(semanticNetwork).Any();
		}

		/// <summary>Returns the first occurrence of the pattern.</summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <param name="isomorphicSearchPattern">Pattern to look for.</param>
		/// <returns>The first match, or <c>null</c> when the pattern does not occur.</returns>
		public static KnowledgeStructure FindFirstMatch(this ISemanticNetwork semanticNetwork, IsomorphicSearchPattern isomorphicSearchPattern)
		{
			return isomorphicSearchPattern.FindMatches(semanticNetwork).FirstOrDefault();
		}
	}
}
