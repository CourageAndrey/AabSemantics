using System.Collections.Generic;
using System.Linq;

using AabSemantics.Concepts;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Mutations;
using AabSemantics.Statements;
using AabSemantics.TestCore;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class StatementSearchPatternTest
	{
		[Test]
		public void GivenStatementFilter_WhenFindMatches_ThenFilter()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept a, b, c, d;
			semanticNetwork.Concepts.Add(a = "a".CreateConcept());
			semanticNetwork.Concepts.Add(b = "b".CreateConcept());
			semanticNetwork.Concepts.Add(c = "c".CreateConcept());
			semanticNetwork.Concepts.Add(d = "d".CreateConcept());
			semanticNetwork.DeclareThat(a).IsAncestorOf(b);
			semanticNetwork.DeclareThat(a).IsAncestorOf(c);
			semanticNetwork.DeclareThat(a).IsAncestorOf(d);

			var statementFilter = new StatementFilter<IsStatement>(statement => statement.Ancestor == a);
			var statementSearchPattern = new StatementSearchPattern<IsStatement>(statementFilter);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(3, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 1 &&
				((IsStatement) m.Knowledge[statementSearchPattern]).Ancestor == a));
		}

		[Test]
		public void GivenConceptFilter_WhenFindMatches_ThenFilter()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept a, b, c, d;
			semanticNetwork.Concepts.Add(a = "a".CreateConcept());
			semanticNetwork.Concepts.Add(b = "b".CreateConcept());
			semanticNetwork.Concepts.Add(c = "c".CreateConcept());
			semanticNetwork.Concepts.Add(d = "d".CreateConcept());
			semanticNetwork.DeclareThat(a).IsAncestorOf(b);
			semanticNetwork.DeclareThat(a).IsAncestorOf(c);
			semanticNetwork.DeclareThat(a).IsAncestorOf(d);

			var conceptSearchPattern = new ConceptSearchPattern(concept => concept.ID == "a");
			var conceptFilters = new[] { new StatementConceptFilter<IsStatement>(statement => statement.Ancestor, conceptSearchPattern) };
			var statementSearchPattern = new StatementSearchPattern<IsStatement>(conceptFilters : conceptFilters);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(3, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 2 &&
				m.Knowledge[statementSearchPattern] is IsStatement &&
				m.Knowledge[conceptSearchPattern] == semanticNetwork.Concepts["a"]));
		}

		[Test]
		public void GivenFiltersStructure_WhenFindMatches_ThenFilter()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept a, b, c, d;
			semanticNetwork.Concepts.Add(a = "a".CreateConcept());
			semanticNetwork.Concepts.Add(b = "b".CreateConcept());
			semanticNetwork.Concepts.Add(c = "c".CreateConcept());
			semanticNetwork.Concepts.Add(d = "d".CreateConcept());
			semanticNetwork.DeclareThat(a).IsAncestorOf(b);
			semanticNetwork.DeclareThat(a).IsAncestorOf(c);
			semanticNetwork.DeclareThat(a).IsAncestorOf(d);

			var statementFilter = new StatementFilter<IsStatement>(statement => statement.Descendant.ID == "b" || statement.Descendant.ID == "d");
			var conceptSearchPattern = new ConceptSearchPattern(concept => concept.ID == "a");
			var conceptFilters = new[] { new StatementConceptFilter<IsStatement>(statement => statement.Ancestor, conceptSearchPattern) };
			var statementSearchPattern = new StatementSearchPattern<IsStatement>(statementFilter, conceptFilters);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(2, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 2 &&
				m.Knowledge[statementSearchPattern] is IsStatement &&
				m.Knowledge[conceptSearchPattern].ID == "a"));
		}

		[Test]
		public void GivenAllFilter_WhenFindMatches_ThenFilter()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default)
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			var allTypedStatements = new List<TestStatement>
			{
				new TestStatement(),
				new TestStatement(),
				new TestStatement(),
				new TestStatement(),
				new TestStatement(),
			};
			foreach (var statement in allTypedStatements)
			{
				semanticNetwork.Statements.Add(statement);
			}

			var searchPattern = StatementSearchPattern<TestStatement>.All;

			// act
			var matches = searchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(allTypedStatements.Count, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SemanticNetwork == semanticNetwork &&
				m.SearchPattern == searchPattern &&
				m.Knowledge.Keys.Single() == searchPattern &&
				allTypedStatements.Contains(m.Knowledge.Values.Single())));
		}
	}
}
