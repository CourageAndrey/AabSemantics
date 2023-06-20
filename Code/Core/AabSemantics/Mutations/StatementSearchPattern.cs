using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Mutations
{
	public class StatementSearchPattern<StatementT> : IsomorphicSearchPattern
		where StatementT : IStatement
	{
		private readonly StatementFilter<StatementT> _statementFilter;
		private readonly IEnumerable<StatementConceptFilter<StatementT>> _conceptFilters;

		public StatementSearchPattern(
			StatementFilter<StatementT> statementFilter = null,
			IEnumerable<StatementConceptFilter<StatementT>> conceptFilters = null)
		{
			_statementFilter = statementFilter ?? (statement => true);

			_conceptFilters = conceptFilters ?? Array.Empty<StatementConceptFilter<StatementT>>();
		}

		public IEnumerable<StatementT> FindStatements(IEnumerable<IStatement> statements)
		{
			return statements.OfType<StatementT>().Where(statement => _statementFilter(statement) && _conceptFilters.All(filter => filter.ConceptFilter.FindConcepts(new[] { filter.ConceptSelector(statement) }).Any()));
		}

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

		public static readonly StatementSearchPattern<StatementT> All = new StatementSearchPattern<StatementT>();
	}
}
