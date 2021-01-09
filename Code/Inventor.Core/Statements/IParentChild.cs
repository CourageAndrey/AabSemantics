using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Statements
{
	public interface IParentChild<out T>
	{
		T Parent { get; }

		T Child { get; }
	}

	public static class ParentChildHelper
	{
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<Statement> statements, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsAllLevels(statements.OfType<RelationshipT>(), item);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<Statement> statements, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenAllLevels(statements.OfType<RelationshipT>(), item);
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<Statement> statements, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsOneLevel(statements.OfType<RelationshipT>(), item);
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<Statement> statements, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenOnLevel(statements.OfType<RelationshipT>(), item);
		}

		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetRelatedAllLevels(relationships, item, GetParentsOneLevel);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetRelatedAllLevels(relationships, item, GetChildrenOnLevel);
		}

		private delegate List<T> GetRelativesDelegate<T, in RelationshipT>(IEnumerable<RelationshipT> relationships, T item);

		private static List<T> GetRelatedAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, GetRelativesDelegate<T, RelationshipT> getRelatives)
		{
			var result = new List<T>();
			var relativesToCheck = new List<T> { item };
			while (relativesToCheck.Count > 0)
			{
				var nextGeneration = relativesToCheck.Aggregate(new List<T>(), (list, relative) => { list.AddRange(getRelatives(relationships, relative)); return list; });
				nextGeneration.RemoveAll(result.Contains);
				relativesToCheck = nextGeneration.Distinct().ToList();
				result.AddRange(relativesToCheck);
			}
			return result;
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return relationships.Where(c => c.Child == item).Select(c => c.Parent).ToList();
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return relationships.Where(c => c.Parent == item).Select(c => c.Child).ToList();
		}
	}
}
