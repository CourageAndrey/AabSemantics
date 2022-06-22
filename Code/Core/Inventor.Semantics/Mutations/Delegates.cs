using System;

namespace Inventor.Semantics.Mutations
{
	public delegate Boolean ConceptFilter(IConcept concept);

	public delegate Boolean StatementFilter<in StatementT>(StatementT statement);

	public delegate IConcept StatementConceptSelector<in StatementT>(StatementT statement);
}
