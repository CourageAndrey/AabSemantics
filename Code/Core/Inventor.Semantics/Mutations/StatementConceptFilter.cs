using System;

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
			if (conceptSelector != null)
			{
				ConceptSelector = conceptSelector;
			}
			else
			{
				throw new ArgumentNullException(nameof(conceptSelector));
			}

			if (conceptFilter != null)
			{
				ConceptFilter = conceptFilter;
			}
			else
			{
				throw new ArgumentNullException(nameof(conceptFilter));
			}
		}
	}
}