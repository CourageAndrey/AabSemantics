using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Mutations;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Mutations
{
	[TestFixture]
	public class JoinSearchPatternTest
	{
		[Test]
		public void GivenNoLeftSearchPattern_WhenTryToCreate_ThenFail()
		{
			// arrange
			var rightSearchPattern = new StatementSearchPattern<IsStatement>();
			var leftConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);
			var rightConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new JoinSearchPattern<IsStatement, IsStatement>(
				null,
				rightSearchPattern,
				JoinType.IntersectJoin,
				leftConceptSelector,
				rightConceptSelector));
		}

		[Test]
		public void GivenNoRightSearchPattern_WhenTryToCreate_ThenFail()
		{
			// arrange
			var leftSearchPattern = new StatementSearchPattern<IsStatement>();
			var leftConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);
			var rightConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new JoinSearchPattern<IsStatement, IsStatement>(
				leftSearchPattern,
				null,
				JoinType.IntersectJoin,
				leftConceptSelector,
				rightConceptSelector));
		}

		[Test]
		public void GivenNoRightLeftConceptSelector_WhenTryToCreate_ThenFail()
		{
			// arrange
			var leftSearchPattern = new StatementSearchPattern<IsStatement>();
			var rightSearchPattern = new StatementSearchPattern<IsStatement>();
			var rightConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new JoinSearchPattern<IsStatement, IsStatement>(
				leftSearchPattern,
				rightSearchPattern,
				JoinType.IntersectJoin,
				null,
				rightConceptSelector));
		}

		[Test]
		public void GivenNoRightConceptSelector_WhenTryToCreate_ThenFail()
		{
			// arrange
			var leftSearchPattern = new StatementSearchPattern<IsStatement>();
			var rightSearchPattern = new StatementSearchPattern<IsStatement>();
			var leftConceptSelector = new StatementConceptSelector<IsStatement>(statement => null);

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new JoinSearchPattern<IsStatement, IsStatement>(
				leftSearchPattern,
				rightSearchPattern,
				JoinType.IntersectJoin,
				leftConceptSelector,
				null));
		}

		[Test]
		public void GivenAllData_WhenIntersectJoin_ThenFindProperData()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();
			var joinSearchPattern = CreateTestSearchPattern(JoinType.IntersectJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(8, matches.Count);
			ValidateInnerJoins(semanticNetwork, matches, joinSearchPattern);
		}

		[Test]
		public void GivenAllData_WhenLeftJoin_ThenFindProperData()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();
			var joinSearchPattern = CreateTestSearchPattern(JoinType.LeftJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(9, matches.Count);
			ValidateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["1"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Left])?.Ancestor == semanticNetwork.Concepts["1"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Right])?.Descendant == null));
		}

		[Test]
		public void GivenAllData_WhenRightJoin_ThenFindProperData()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();
			var joinSearchPattern = CreateTestSearchPattern(JoinType.RightJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(9, matches.Count);
			ValidateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["10"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Left])?.Ancestor == null &&
				((IsStatement) m.Knowledge[joinSearchPattern.Right])?.Descendant == semanticNetwork.Concepts["10"]));
		}

		[Test]
		public void GivenAllData_WhenFullJoin_ThenFindProperData()
		{
			// arrange
			var semanticNetwork = CreateTestSemanticNetwork();
			var joinSearchPattern = CreateTestSearchPattern(JoinType.FullJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(10, matches.Count);
			ValidateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["1"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Left])?.Ancestor == semanticNetwork.Concepts["1"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Right])?.Descendant == null));
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["10"] &&
				((IsStatement) m.Knowledge[joinSearchPattern.Left])?.Ancestor == null &&
				((IsStatement) m.Knowledge[joinSearchPattern.Right])?.Descendant == semanticNetwork.Concepts["10"]));
		}

		private const int _numbersCount = 10;

		private static ISemanticNetwork CreateTestSemanticNetwork()
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			for (int i = 1; i <= _numbersCount; i++)
			{
				semanticNetwork.Concepts.Add(i.CreateConceptByObject());
			}

			for (int i = 1; i < _numbersCount; i++)
			{
				semanticNetwork.DeclareThat(semanticNetwork.Concepts[i.ToString()]).IsAncestorOf(semanticNetwork.Concepts[(i+1).ToString()]);
			}

			return semanticNetwork;
		}

		private static JoinSearchPattern<IsStatement, IsStatement> CreateTestSearchPattern(JoinType joinType)
		{
			return new JoinSearchPattern<IsStatement, IsStatement>(
				new StatementSearchPattern<IsStatement>(),
				new StatementSearchPattern<IsStatement>(),
				joinType,
				comparison => comparison.Ancestor,
				comparison => comparison.Descendant);
		}

		private static void ValidateInnerJoins(ISemanticNetwork semanticNetwork, List<KnowledgeStructure> matches, JoinSearchPattern<IsStatement, IsStatement> joinSearchPattern)
		{
			Assert.IsTrue(matches.All(m =>
			{
				var left = m.Knowledge[joinSearchPattern.Left] as IsStatement;
				var right = m.Knowledge[joinSearchPattern.Right] as IsStatement;
				if (left == null || right == null) return true;

				return	m.SearchPattern == joinSearchPattern &&
						m.SemanticNetwork == semanticNetwork &&
						m.Knowledge.Count == 3 &&
						m.Knowledge.ContainsKey(joinSearchPattern) &&
						left.Ancestor == right.Descendant &&
						int.Parse(left.Descendant.ID) - int.Parse(right.Ancestor.ID) == 2;
			}));
		}
	}
}
