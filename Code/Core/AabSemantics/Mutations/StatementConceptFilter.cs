using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>
	/// Constrains one role of a statement: which concept to look at, and which pattern that
	/// concept must match. The pattern doubles as the binding key in the resulting
	/// <see cref="KnowledgeStructure"/>, so reusing one instance for two roles collapses them.
	/// </summary>
	/// <typeparam name="StatementT">Statement type constrained.</typeparam>
	public class StatementConceptFilter<StatementT>
		where StatementT : IStatement
	{
		/// <summary>Picks the concept this filter applies to.</summary>
		public StatementConceptSelector<StatementT> ConceptSelector
		{ get; }

		/// <summary>Pattern the selected concept must match.</summary>
		public ConceptSearchPattern ConceptFilter
		{ get; }

		/// <summary>Creates a role constraint.</summary>
		/// <param name="conceptSelector">Picks the concept to constrain.</param>
		/// <param name="conceptFilter">Pattern the selected concept must match.</param>
		/// <exception cref="System.ArgumentNullException">Any argument is <c>null</c>.</exception>
		public StatementConceptFilter(StatementConceptSelector<StatementT> conceptSelector, ConceptSearchPattern conceptFilter)
		{
			ConceptSelector = conceptSelector.EnsureNotNull(nameof(conceptSelector));
			ConceptFilter = conceptFilter.EnsureNotNull(nameof(conceptFilter));
		}
	}
}