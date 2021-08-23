using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface IParentChild<out T>
	{
		T Parent { get; }

		T Child { get; }
	}

	public static class ParentChildHelper
	{
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsAllLevels(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenAllLevels(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsOneLevel(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenOnLevel(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static ICollection<IStatement> FindPath<T>(this IEnumerable<IStatement> statements, Type statementType, T parent, T child)
			where T : class
		{
			var typedStatements = statements.OfType<IParentChild<T>>().Where(statement => statement.GetType() == statementType).ToList();

			// search up (child > parent), because search tree has to be smaller in this case
			var pathsToCheck = typedStatements.Where(statement => statement.Child == child).Select(statement => new List<IParentChild<T>> { statement }).ToList();
			while (pathsToCheck.Any())
			{
				var nextStep = new List<List<IParentChild<T>>>();
				foreach (var path in pathsToCheck)
				{
					var lastParent = path.Last().Parent;
					if (lastParent == parent)
					{
						return path.OfType<IStatement>().ToList();
					}
					else if (!path.Select(statement => statement.Child).Contains(lastParent))
					{
						nextStep.AddRange(typedStatements.Where(statement => statement.Child == lastParent).Select(statement => new List<IParentChild<T>>(path) { statement }));
					}
				}
				pathsToCheck = nextStep;
			}

			return Array.Empty<IStatement>();
		}

		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetRelatedAllLevels(relationships, item, involvedRelationships, GetParentsOneLevel);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetRelatedAllLevels(relationships, item, involvedRelationships, GetChildrenOnLevel);
		}

		private delegate List<T> GetRelativesDelegate<T, RelationshipT>(IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null);

		private static List<T> GetRelatedAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships, GetRelativesDelegate<T, RelationshipT> getRelatives)
		{
			var result = new List<T>();
			var relativesToCheck = new List<T> { item };
			while (relativesToCheck.Count > 0)
			{
				var nextGeneration = relativesToCheck.Aggregate(new List<T>(), (list, relative) => { list.AddRange(getRelatives(relationships, relative, involvedRelationships)); return list; });
				nextGeneration.RemoveAll(result.Contains);
				relativesToCheck = nextGeneration.Distinct().ToList();
				result.AddRange(relativesToCheck);
			}
			return result;
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var foundRelationships = relationships.Where(c => c.Child == item).ToList();
			if (involvedRelationships != null)
			{
				involvedRelationships.AddRange(foundRelationships);
			}
			return foundRelationships.Select(c => c.Parent).ToList();
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var foundRelationships = relationships.Where(c => c.Parent == item).ToList();
			if (involvedRelationships != null)
			{
				involvedRelationships.AddRange(foundRelationships);
			}
			return foundRelationships.Select(c => c.Child).ToList();
		}
	}
}
