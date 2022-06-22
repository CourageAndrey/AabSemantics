using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Mutations
{
	public enum JoinType
	{
		IntersectJoin,
		LeftJoin,
		RightJoin,
		FullJoin,
	}

	public class JoinSearchPattern<LeftStatementT, RightStatementT> : IsomorphicSearchPattern
		where LeftStatementT : class, IStatement
		where RightStatementT : class, IStatement
	{
		public StatementSearchPattern<LeftStatementT> Left
		{ get; }

		public StatementSearchPattern<RightStatementT> Right
		{ get; }

		public JoinType JoinType
		{ get; }

		private readonly StatementConceptSelector<LeftStatementT> _leftConceptSelector;
		private readonly StatementConceptSelector<RightStatementT> _rightConceptSelector;

		public JoinSearchPattern(
			StatementSearchPattern<LeftStatementT> left,
			StatementSearchPattern<RightStatementT> right,
			JoinType joinType,
			StatementConceptSelector<LeftStatementT> leftConceptSelector,
			StatementConceptSelector<RightStatementT> rightConceptSelector)
		{
			if (left != null)
			{
				Left = left;
			}
			else
			{
				throw new ArgumentNullException(nameof(left));
			}

			if (right != null)
			{
				Right = right;
			}
			else
			{
				throw new ArgumentNullException(nameof(right));
			}

			JoinType = joinType;

			if (leftConceptSelector != null)
			{
				_leftConceptSelector = leftConceptSelector;
			}
			else
			{
				throw new ArgumentNullException(nameof(leftConceptSelector));
			}

			if (rightConceptSelector != null)
			{
				_rightConceptSelector = rightConceptSelector;
			}
			else
			{
				throw new ArgumentNullException(nameof(rightConceptSelector));
			}
		}

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
