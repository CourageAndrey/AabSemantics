using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Mutations
{
	/// <summary>
	/// Pattern matching statements of one type, optionally constraining the concepts they relate.
	/// </summary>
	/// <typeparam name="StatementT">Statement type to match.</typeparam>
	public class StatementSearchPattern<StatementT> : IsomorphicSearchPattern
		where StatementT : IStatement
	{
		private readonly StatementFilter<StatementT> _statementFilter;
		private readonly IEnumerable<StatementConceptFilter<StatementT>> _conceptFilters;

		/// <summary>Creates a statement pattern; omitting both arguments matches every statement of the type.</summary>
		/// <param name="statementFilter">Predicate the statement must satisfy; <c>null</c> accepts any.</param>
		/// <param name="conceptFilters">Per-role concept constraints, all of which must hold; <c>null</c> means none.</param>
		public StatementSearchPattern(
			StatementFilter<StatementT> statementFilter = null,
			IEnumerable<StatementConceptFilter<StatementT>> conceptFilters = null)
		{
			_statementFilter = statementFilter ?? (statement => true);

			_conceptFilters = conceptFilters ?? Array.Empty<StatementConceptFilter<StatementT>>();
		}

		/// <summary>Filters a statement sequence by the pattern's type, predicate and role constraints.</summary>
		/// <param name="statements">Statements to filter; those of other types are ignored.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public IEnumerable<StatementT> FindStatements(IEnumerable<IStatement> statements)
		{
			return statements.OfType<StatementT>().Where(statement => _statementFilter(statement) && _conceptFilters.All(filter => filter.ConceptFilter.FindConcepts(new[] { filter.ConceptSelector(statement) }).Any()));
		}

		/// <summary>
		/// Finds every matching statement in a network. Each match binds the statement to this
		/// pattern and every constrained concept to its own concept pattern.
		/// </summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <returns>Lazily evaluated matches.</returns>
		public override IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork)
		{
			return FindStatements(semanticNetwork.Statements).Select(statement => new KnowledgeStructure(
				semanticNetwork,
				this,
				new Dictionary<IsomorphicSearchPattern, IKnowledge>(_conceptFilters.ToDictionary(
					filter => filter.ConceptFilter as IsomorphicSearchPattern,
					filter => filter.ConceptSelector(statement) as IKnowledge))
					{ { this, statement } }));
		}

		/// <summary>Pattern matching every statement of the type, without constraints.</summary>
		public static readonly StatementSearchPattern<StatementT> All = new StatementSearchPattern<StatementT>();
	}
}
