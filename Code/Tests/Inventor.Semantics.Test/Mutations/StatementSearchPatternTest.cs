using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Mathematics.Concepts;
using Inventor.Semantics.Mathematics.Statements;
using Inventor.Semantics.Mutations;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Mutations
{
	[TestFixture]
	public class StatementSearchPatternTest
	{
		[Test]
		public void FilteringByStatementFilterItsef()
		{
			// arrange
			var semanticNetwork = new TestSemanticNetwork(Language.Default);

			var statementFilter = new StatementFilter<ComparisonStatement>(comparison => comparison.ComparisonSign == ComparisonSigns.IsGreaterThanOrEqualTo);
			var statementSearchPattern = new StatementSearchPattern<ComparisonStatement>(statementFilter);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork.SemanticNetwork).ToList();

			// assert
			Assert.AreEqual(3, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork.SemanticNetwork &&
				m.Knowledge.Count == 1 &&
				((ComparisonStatement) m.Knowledge[statementSearchPattern]).ComparisonSign == ComparisonSigns.IsGreaterThanOrEqualTo));
		}

		[Test]
		public void FilteringByConceptFilters()
		{
			// arrange
			var semanticNetwork = new TestSemanticNetwork(Language.Default);

			var conceptSearchPattern = new ConceptSearchPattern(number => number.ID.Contains("3"));
			var conceptFilters = new[] { new StatementConceptFilter<ComparisonStatement>(comparison => comparison.LeftValue, conceptSearchPattern) };
			var statementSearchPattern = new StatementSearchPattern<ComparisonStatement>(conceptFilters : conceptFilters);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork.SemanticNetwork).ToList();

			// assert
			Assert.AreEqual(3, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork.SemanticNetwork &&
				m.Knowledge.Count == 2 &&
				m.Knowledge[statementSearchPattern] is ComparisonStatement &&
				(m.Knowledge[conceptSearchPattern] == semanticNetwork.Number3 || m.Knowledge[conceptSearchPattern] == semanticNetwork.Number3or4)));
		}

		[Test]
		public void FilteringByAllFilters()
		{
			// arrange
			var semanticNetwork = new TestSemanticNetwork(Language.Default);

			var statementFilter = new StatementFilter<ComparisonStatement>(comparison => comparison.ComparisonSign == ComparisonSigns.IsGreaterThanOrEqualTo);
			var conceptSearchPattern = new ConceptSearchPattern(number => number.ID.Contains("3"));
			var conceptFilters = new[] { new StatementConceptFilter<ComparisonStatement>(comparison => comparison.LeftValue, conceptSearchPattern) };
			var statementSearchPattern = new StatementSearchPattern<ComparisonStatement>(statementFilter, conceptFilters);

			// act
			var matches = statementSearchPattern.FindMatches(semanticNetwork.SemanticNetwork).ToList();

			// assert
			Assert.AreEqual(2, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SearchPattern == statementSearchPattern &&
				m.SemanticNetwork == semanticNetwork.SemanticNetwork &&
				m.Knowledge.Count == 2));
			Assert.AreEqual(1, matches.Count(m =>
				m.Knowledge[statementSearchPattern] is ComparisonStatement &&
				m.Knowledge[conceptSearchPattern] == semanticNetwork.Number3));
			Assert.AreEqual(1, matches.Count(m =>
				m.Knowledge[statementSearchPattern] is ComparisonStatement &&
				m.Knowledge[conceptSearchPattern] == semanticNetwork.Number3or4));
		}

		[Test]
		public void AllFilterWorksTheSameAsOfType()
		{
			// arrange
			var semanticNetwork = new TestSemanticNetwork(Language.Default).SemanticNetwork;
			var groupStatements = semanticNetwork.Statements.OfType<GroupStatement>().ToList();

			var searchPattern = StatementSearchPattern<GroupStatement>.All;

			// act
			var matches = searchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(groupStatements.Count, matches.Count);
			Assert.IsTrue(matches.All(m =>
				m.SemanticNetwork == semanticNetwork &&
				m.SearchPattern == searchPattern &&
				m.Knowledge.Keys.Single() == searchPattern &&
				groupStatements.Contains(m.Knowledge.Values.Single())));
		}
	}
}
