using System;
using System.Collections.Generic;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Mutations
{
	public class KnowledgeStructure
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IsomorphicSearchPattern SearchPattern
		{ get; }

		public IDictionary<IsomorphicSearchPattern, IKnowledge> Knowledge
		{ get; }

		public KnowledgeStructure(ISemanticNetwork semanticNetwork, IsomorphicSearchPattern searchPattern, IDictionary<IsomorphicSearchPattern, IKnowledge> knowledge)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			SearchPattern = searchPattern.EnsureNotNull(nameof(searchPattern));
			Knowledge = knowledge.EnsureNotNull(nameof(knowledge));
		}
	}
}
