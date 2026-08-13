using System;

namespace AabSemantics.Mutations
{
	/// <summary>Decides whether a concept matches a search pattern.</summary>
	/// <param name="concept">Concept to test.</param>
	/// <returns><c>true</c> if the concept matches.</returns>
	public delegate Boolean ConceptFilter(IConcept concept);

	/// <summary>Decides whether a statement matches a search pattern.</summary>
	/// <typeparam name="StatementT">Statement type tested.</typeparam>
	/// <param name="statement">Statement to test.</param>
	/// <returns><c>true</c> if the statement matches.</returns>
	public delegate Boolean StatementFilter<in StatementT>(StatementT statement);

	/// <summary>Picks one of the concepts a statement relates, identifying its role in the pattern.</summary>
	/// <typeparam name="StatementT">Statement type read.</typeparam>
	/// <param name="statement">Statement to read.</param>
	/// <returns>The selected concept.</returns>
	public delegate IConcept StatementConceptSelector<in StatementT>(StatementT statement);
}
