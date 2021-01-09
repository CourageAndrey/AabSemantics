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
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsAllLevels(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenAllLevels(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetParentsOneLevel(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<Statement> statements, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return GetChildrenOnLevel(statements.OfType<RelationshipT>(), concept);
		}

		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var result = new List<T>();
			var parentsToCheck = new List<T> { concept };
			while (parentsToCheck.Count > 0)
			{
				var nextGeneration = parentsToCheck.Aggregate(new List<T>(), (list, parent) => { list.AddRange(GetParentsOneLevel(relationships, parent)); return list; });
				nextGeneration.RemoveAll(result.Contains);
				parentsToCheck = nextGeneration.Distinct().ToList();
				result.AddRange(parentsToCheck);
			}
			return result;
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var result = new List<T>();
			var childrenToCheck = new List<T> { concept };
			while (childrenToCheck.Count > 0)
			{
				var nextGeneration = childrenToCheck.Aggregate(new List<T>(), (list, child) => { list.AddRange(GetChildrenOnLevel(relationships, child)); return list; });
				nextGeneration.RemoveAll(result.Contains);
				childrenToCheck = nextGeneration.Distinct().ToList();
				result.AddRange(childrenToCheck);
			}
			return result;
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return relationships.Where(c => c.Child == concept).Select(c => c.Parent).ToList();
		}

		public static List<T> GetChildrenOnLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T concept)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return relationships.Where(c => c.Parent == concept).Select(c => c.Child).ToList();
		}
	}
}
