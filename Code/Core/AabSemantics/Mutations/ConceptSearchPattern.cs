using System.Collections.Generic;
using System.Linq;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>Pattern matching individual concepts by a predicate.</summary>
	public class ConceptSearchPattern : IsomorphicSearchPattern
	{
		private readonly ConceptFilter _filter;

		/// <summary>Creates a concept pattern.</summary>
		/// <param name="filter">Predicate a concept must satisfy.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="filter"/> is <c>null</c>.</exception>
		public ConceptSearchPattern(ConceptFilter filter)
		{
			_filter = filter.EnsureNotNull(nameof(filter));
		}

		/// <summary>Filters a concept sequence by the pattern's predicate.</summary>
		/// <param name="concepts">Concepts to filter.</param>
		/// <returns>Lazily evaluated matching concepts.</returns>
		public IEnumerable<IConcept> FindConcepts(IEnumerable<IConcept> concepts)
		{
			return concepts.Where(concept => _filter(concept));
		}

		/// <summary>Finds every matching concept in a network, one match per concept.</summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <returns>Lazily evaluated matches.</returns>
		public override IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork)
		{
			return FindConcepts(semanticNetwork.Concepts).Select(concept => new KnowledgeStructure(
				semanticNetwork,
				this,
				new Dictionary<IsomorphicSearchPattern, IKnowledge> { { this, concept } }));
		}

		/// <summary>Pattern matching every concept.</summary>
		public static readonly ConceptSearchPattern All = new ConceptSearchPattern(concept => true);
	}
}
