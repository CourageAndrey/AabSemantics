using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Mutations
{
	public class ConceptSearchPattern : IsomorphicSearchPattern
	{
		private readonly ConceptFilter _filter;

		public ConceptSearchPattern(ConceptFilter filter)
		{
			_filter = filter;
		}

		public IEnumerable<IConcept> FindConcepts(IEnumerable<IConcept> concepts)
		{
			return concepts.Where(concept => _filter(concept));
		}

		public override IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork)
		{
			return FindConcepts(semanticNetwork.Concepts).Select(concept => new KnowledgeStructure(
				semanticNetwork,
				this,
				new Dictionary<IsomorphicSearchPattern, IKnowledge> { { this, concept } }));
		}

		public static readonly ConceptSearchPattern All = new ConceptSearchPattern(concept => true);
	}
}
