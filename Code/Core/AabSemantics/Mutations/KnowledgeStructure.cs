using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>
	/// A concrete match of a search pattern against a semantic network: which real concept or
	/// statement each part of the pattern bound to. A production reads this to know what to act on.
	/// </summary>
	public class KnowledgeStructure
	{
		/// <summary>Network the match was found in.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>Pattern that was matched.</summary>
		public IsomorphicSearchPattern SearchPattern
		{ get; }

		/// <summary>Which knowledge item each part of the pattern bound to.</summary>
		public IDictionary<IsomorphicSearchPattern, IKnowledge> Knowledge
		{ get; }

		/// <summary>Creates a match.</summary>
		/// <param name="semanticNetwork">Network the match was found in.</param>
		/// <param name="searchPattern">Pattern that was matched.</param>
		/// <param name="knowledge">Bindings from pattern parts to knowledge items.</param>
		/// <exception cref="System.ArgumentNullException">Any argument is <c>null</c>.</exception>
		public KnowledgeStructure(ISemanticNetwork semanticNetwork, IsomorphicSearchPattern searchPattern, IDictionary<IsomorphicSearchPattern, IKnowledge> knowledge)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			SearchPattern = searchPattern.EnsureNotNull(nameof(searchPattern));
			Knowledge = knowledge.EnsureNotNull(nameof(knowledge));
		}
	}
}
