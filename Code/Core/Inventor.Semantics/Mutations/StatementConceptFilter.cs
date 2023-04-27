using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Mutations
{
	public class StatementConceptFilter<StatementT>
		where StatementT : IStatement
	{
		public StatementConceptSelector<StatementT> ConceptSelector
		{ get; }

		public ConceptSearchPattern ConceptFilter
		{ get; }

		public StatementConceptFilter(StatementConceptSelector<StatementT> conceptSelector, ConceptSearchPattern conceptFilter)
		{
			ConceptSelector = conceptSelector.EnsureNotNull(nameof(conceptSelector));
			ConceptFilter = conceptFilter.EnsureNotNull(nameof(conceptFilter));
		}
	}
}