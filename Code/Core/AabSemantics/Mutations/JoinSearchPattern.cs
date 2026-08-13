using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>How unmatched statements are treated when joining two patterns, as in SQL joins.</summary>
	public enum JoinType
	{
		/// <summary>Only pairs where both sides share a concept.</summary>
		IntersectJoin,

		/// <summary>Every left statement, paired with a right one where a shared concept exists.</summary>
		LeftJoin,

		/// <summary>Every right statement, paired with a left one where a shared concept exists.</summary>
		RightJoin,

		/// <summary>Every statement from both sides, paired up wherever a shared concept exists.</summary>
		FullJoin,
	}

	/// <summary>
	/// Pattern matching pairs of statements that meet at a common concept — the way an
	/// "A is B" and a "B is C" statement combine to justify "A is C".
	/// </summary>
	/// <typeparam name="LeftStatementT">Statement type on the left of the join.</typeparam>
	/// <typeparam name="RightStatementT">Statement type on the right of the join.</typeparam>
	public class JoinSearchPattern<LeftStatementT, RightStatementT> : IsomorphicSearchPattern
		where LeftStatementT : class, IStatement
		where RightStatementT : class, IStatement
	{
		/// <summary>Pattern the left statements must match.</summary>
		public StatementSearchPattern<LeftStatementT> Left
		{ get; }

		/// <summary>Pattern the right statements must match.</summary>
		public StatementSearchPattern<RightStatementT> Right
		{ get; }

		/// <summary>How unmatched statements are treated.</summary>
		public JoinType JoinType
		{ get; }

		private readonly StatementConceptSelector<LeftStatementT> _leftConceptSelector;
		private readonly StatementConceptSelector<RightStatementT> _rightConceptSelector;

		/// <summary>Creates a join pattern.</summary>
		/// <param name="left">Pattern the left statements must match.</param>
		/// <param name="right">Pattern the right statements must match.</param>
		/// <param name="joinType">How unmatched statements are treated.</param>
		/// <param name="leftConceptSelector">Picks the concept a left statement joins on.</param>
		/// <param name="rightConceptSelector">Picks the concept a right statement joins on.</param>
		/// <exception cref="ArgumentNullException">Any argument except <paramref name="joinType"/> is <c>null</c>.</exception>
		public JoinSearchPattern(
			StatementSearchPattern<LeftStatementT> left,
			StatementSearchPattern<RightStatementT> right,
			JoinType joinType,
			StatementConceptSelector<LeftStatementT> leftConceptSelector,
			StatementConceptSelector<RightStatementT> rightConceptSelector)
		{
			Left = left.EnsureNotNull(nameof(left));
			Right = right.EnsureNotNull(nameof(right));
			JoinType = joinType;
			_leftConceptSelector = leftConceptSelector.EnsureNotNull(nameof(leftConceptSelector));
			_rightConceptSelector = rightConceptSelector.EnsureNotNull(nameof(rightConceptSelector));
		}

		/// <summary>
		/// Finds every pair of statements sharing a join concept. Depending on
		/// <see cref="JoinType"/>, a match may have a <c>null</c> statement on one side.
		/// </summary>
		/// <param name="semanticNetwork">Network to search.</param>
		/// <returns>
		/// Lazily evaluated matches, each binding the left pattern, the right pattern and this
		/// pattern to the shared concept.
		/// </returns>
		public override IEnumerable<KnowledgeStructure> FindMatches(ISemanticNetwork semanticNetwork)
		{
			var leftStatements = new Dictionary<IConcept, ICollection<LeftStatementT>>();
			foreach (var statement in Left.FindStatements(semanticNetwork.Statements))
			{
				var concept = _leftConceptSelector(statement);

				ICollection<LeftStatementT> statements;
				if (!leftStatements.TryGetValue(concept, out statements))
				{
					leftStatements[concept] = statements = new List<LeftStatementT>();
				}

				statements.Add(statement);
			}

			var rightStatements = new Dictionary<IConcept, ICollection<RightStatementT>>();
			foreach (var statement in Right.FindStatements(semanticNetwork.Statements))
			{
				var concept = _rightConceptSelector(statement);

				ICollection<RightStatementT> statements;
				if (!rightStatements.TryGetValue(concept, out statements))
				{
					rightStatements[concept] = statements = new List<RightStatementT>();
				}

				statements.Add(statement);
			}

			foreach (var combination in _joinFunctions[JoinType](leftStatements, rightStatements))
			{
				var knowledge = new Dictionary<IsomorphicSearchPattern, IKnowledge>
				{
					{ Left, combination.Item1 },
					{ Right, combination.Item2 },
					{ this, combination.Item3 },
				};

				yield return new KnowledgeStructure(semanticNetwork, this, knowledge);
			}
		}

		private static IEnumerable<Tuple<LeftStatementT, RightStatementT, IConcept>> performIntersectJoin(
			IDictionary<IConcept, ICollection<LeftStatementT>> leftStatements,
			IDictionary<IConcept, ICollection<RightStatementT>> rightStatements)
		{
			foreach (var joinConcept in leftStatements.Keys.Intersect(rightStatements.Keys))
			{
				foreach (var leftStatement in leftStatements[joinConcept])
				{
					foreach (var rightStatement in rightStatements[joinConcept])
					{
						yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, rightStatement, joinConcept);
					}
				}
			}
		}

		private static IEnumerable<Tuple<LeftStatementT, RightStatementT, IConcept>> performLeftJoin(
			IDictionary<IConcept, ICollection<LeftStatementT>> leftStatements,
			IDictionary<IConcept, ICollection<RightStatementT>> rightStatements)
		{
			foreach (var sourceStatements in leftStatements)
			{
				var joinConcept = sourceStatements.Key;

				ICollection<RightStatementT> extensionStatements;
				if (rightStatements.TryGetValue(joinConcept, out extensionStatements))
				{
					foreach (var leftStatement in sourceStatements.Value)
					{
						foreach (var rightStatement in extensionStatements)
						{
							yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, rightStatement, joinConcept);
						}
					}
				}
				else
				{
					foreach (var leftStatement in sourceStatements.Value)
					{
						yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, null, joinConcept);
					}
				}
			}
		}

		private static IEnumerable<Tuple<LeftStatementT, RightStatementT, IConcept>> performRightJoin(
			IDictionary<IConcept, ICollection<LeftStatementT>> leftStatements,
			IDictionary<IConcept, ICollection<RightStatementT>> rightStatements)
		{
			foreach (var sourceStatements in rightStatements)
			{
				var joinConcept = sourceStatements.Key;

				ICollection<LeftStatementT> extensionStatements;
				if (leftStatements.TryGetValue(joinConcept, out extensionStatements))
				{
					foreach (var rightStatement in sourceStatements.Value)
					{
						foreach (var leftStatement in extensionStatements)
						{
							yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, rightStatement, joinConcept);
						}
					}
				}
				else
				{
					foreach (var rightStatement in sourceStatements.Value)
					{
						yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(null, rightStatement, joinConcept);
					}
				}
			}
		}

		private static IEnumerable<Tuple<LeftStatementT, RightStatementT, IConcept>> performFullJoin(
			IDictionary<IConcept, ICollection<LeftStatementT>> leftStatements,
			IDictionary<IConcept, ICollection<RightStatementT>> rightStatements)
		{
			foreach (var joinConcept in leftStatements.Keys.Union(rightStatements.Keys))
			{
				ICollection<LeftStatementT> lStatements;
				if (!leftStatements.TryGetValue(joinConcept, out lStatements))
				{
					lStatements = null;
				}

				ICollection<RightStatementT> rStatements;
				if (!rightStatements.TryGetValue(joinConcept, out rStatements))
				{
					rStatements = null;
				}

				if (lStatements != null)
				{
					if (rStatements != null)
					{
						foreach (var leftStatement in lStatements)
						{
							foreach (var rightStatement in rStatements)
							{
								yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, rightStatement, joinConcept);
							}
						}
					}
					else
					{
						foreach (var leftStatement in lStatements)
						{
							yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(leftStatement, null, joinConcept);
						}
					}
				}
				else
				{
					foreach (var rightStatement in rStatements) // rStatements is not null here
					{
						yield return new Tuple<LeftStatementT, RightStatementT, IConcept>(null, rightStatement, joinConcept);
					}
				}
			}
		}

		private delegate IEnumerable<Tuple<LeftStatementT, RightStatementT, IConcept>> JoinFunction(
			IDictionary<IConcept, ICollection<LeftStatementT>> leftStatements,
			IDictionary<IConcept, ICollection<RightStatementT>> rightStatements);

		private static readonly IDictionary<JoinType, JoinFunction> _joinFunctions = new Dictionary<JoinType, JoinFunction>
		{
			{ JoinType.IntersectJoin, performIntersectJoin },
			{ JoinType.LeftJoin, performLeftJoin },
			{ JoinType.RightJoin, performRightJoin },
			{ JoinType.FullJoin, performFullJoin },
		};
	}
}
