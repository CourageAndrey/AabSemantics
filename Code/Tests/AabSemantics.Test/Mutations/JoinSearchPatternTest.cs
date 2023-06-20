using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Mutations;
using AabSemantics.Statements;

namespace AabSemantics.Test.Mutations
{
	[TestFixture]
	public class JoinSearchPatternTest
	{
		[Test]
		public void ImpossibleToCreateWithoutLeftSearchPattern()
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
		public void ImpossibleToCreateWithoutRightSearchPattern()
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
		public void ImpossibleToCreateWithoutRightLeftConceptSelector()
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
		public void ImpossibleToCreateWithoutRightConceptSelector()
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
		public void GivenAllDataWhenIntersectJoinThenFindProperData()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();
			var joinSearchPattern = createTestSearchPattern(JoinType.IntersectJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(8, matches.Count);
			validateInnerJoins(semanticNetwork, matches, joinSearchPattern);
		}

		[Test]
		public void GivenAllDataWhenLeftJoinThenFindProperData()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();
			var joinSearchPattern = createTestSearchPattern(JoinType.LeftJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(9, matches.Count);
			validateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["10"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Left])?.LeftValue == semanticNetwork.Concepts["9"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Right])?.RightValue == null));
		}

		[Test]
		public void GivenAllDataWhenRightJoinThenFindProperData()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();
			var joinSearchPattern = createTestSearchPattern(JoinType.RightJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(9, matches.Count);
			validateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["1"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Left])?.LeftValue == null &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Right])?.RightValue == semanticNetwork.Concepts["2"]));
		}

		[Test]
		public void GivenAllDataWhenFullJoinThenFindProperData()
		{
			// arrange
			var semanticNetwork = createTestSemanticNetwork();
			var joinSearchPattern = createTestSearchPattern(JoinType.FullJoin);

			// act
			var matches = joinSearchPattern.FindMatches(semanticNetwork).ToList();

			// assert
			Assert.AreEqual(10, matches.Count);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["10"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Left])?.LeftValue == semanticNetwork.Concepts["9"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Right])?.RightValue == null));
			validateInnerJoins(semanticNetwork, matches, joinSearchPattern);
			Assert.AreEqual(1, matches.Count(m =>
				m.SearchPattern == joinSearchPattern &&
				m.SemanticNetwork == semanticNetwork &&
				m.Knowledge.Count == 3 &&
				m.Knowledge[joinSearchPattern] == semanticNetwork.Concepts["1"] &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Left])?.LeftValue == null &&
				((ComparisonStatement) m.Knowledge[joinSearchPattern.Right])?.RightValue == semanticNetwork.Concepts["2"]));
		}

		private const int _numbersCount = 10;

		private static ISemanticNetwork createTestSemanticNetwork()
		{
			var semanticNetwork = new SemanticNetwork(Language.Default);

			for (int i = 1; i <= _numbersCount; i++)
			{
				IConcept concept;
				semanticNetwork.Concepts.Add(concept = i.CreateConcept());
				concept.WithAttribute(IsValueAttribute.Value);
			}

			for (int i = 1; i < _numbersCount; i++)
			{
				semanticNetwork.DeclareThat(semanticNetwork.Concepts[i.ToString()]).IsLessThan(semanticNetwork.Concepts[(i+1).ToString()]);
			}

			return semanticNetwork;
		}

		private static JoinSearchPattern<ComparisonStatement, ComparisonStatement> createTestSearchPattern(JoinType joinType)
		{
			return new JoinSearchPattern<ComparisonStatement, ComparisonStatement>(
				new StatementSearchPattern<ComparisonStatement>(),
				new StatementSearchPattern<ComparisonStatement>(),
				joinType,
				comparison => comparison.RightValue,
				comparison => comparison.LeftValue);
		}

		private static void validateInnerJoins(ISemanticNetwork semanticNetwork, List<KnowledgeStructure> matches, JoinSearchPattern<ComparisonStatement, ComparisonStatement> joinSearchPattern)
		{
			for (int i = 1; i <= 8; i++)
			{
				Assert.AreEqual(1, matches.Count(m => validateJoin(
					semanticNetwork,
					joinSearchPattern,
					m,
					semanticNetwork.Concepts[i.ToString()],
					semanticNetwork.Concepts[(i + 1).ToString()],
					semanticNetwork.Concepts[(i + 2).ToString()])));
			}
		}

		private static bool validateJoin(
			ISemanticNetwork semanticNetwork,
			JoinSearchPattern<ComparisonStatement, ComparisonStatement> joinSearchPattern,
			KnowledgeStructure knowledgeStructure,
			IConcept leftConcept,
			IConcept joinConcept,
			IConcept rightConcept)
		{
			return	knowledgeStructure.SearchPattern == joinSearchPattern &&
					knowledgeStructure.SemanticNetwork == semanticNetwork &&
					knowledgeStructure.Knowledge.Count == 3 &&
					knowledgeStructure.Knowledge[joinSearchPattern] == joinConcept &&
					((ComparisonStatement) knowledgeStructure.Knowledge[joinSearchPattern.Left]).LeftValue == leftConcept &&
					((ComparisonStatement) knowledgeStructure.Knowledge[joinSearchPattern.Right]).RightValue == rightConcept;
		}
	}
}
