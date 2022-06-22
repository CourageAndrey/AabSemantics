using System;
using System.Collections.Generic;

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
			if (semanticNetwork != null)
			{
				SemanticNetwork = semanticNetwork;
			}
			else
			{
				throw new ArgumentNullException(nameof(semanticNetwork));
			}

			if (searchPattern != null)
			{
				SearchPattern = searchPattern;
			}
			else
			{
				throw new ArgumentNullException(nameof(searchPattern));
			}

			if (knowledge != null)
			{
				Knowledge = knowledge;
			}
			else
			{
				throw new ArgumentNullException(nameof(knowledge));
			}
		}
	}
}
