using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	public interface IParentChild<out T>
	{
		T Parent { get; }

		T Child { get; }
	}

	public class ParentChild<T>
	{
		public T Value
		{ get; }

		public ParentChild<T> Parent
		{ get; private set; }

		public ICollection<ParentChild<T>> Children
		{ get; }

		public ParentChild(T value, ParentChild<T> parent = null, IEnumerable<ParentChild<T>> children = null)
		{
			Value = value;

			SetParent(parent);

			Children = new List<ParentChild<T>>();
			SetChildren(children);
		}

		public void SetParent(ParentChild<T> parent)
		{
			Parent = parent;

			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}

		public void SetChildren(IEnumerable<ParentChild<T>> children)
		{
			Children.Clear();

			if (children != null)
			{
				foreach (var child in children)
				{
					Children.Add(child);
					child.Parent = this;
				}
			}
		}
	}

	public static class ParentChildHelper
	{
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsAllLevelsAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		public static async Task<List<T>> GetParentsAllLevelsAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetParentsAllLevelsAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenAllLevelsAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		public static async Task<List<T>> GetChildrenAllLevelsAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenAllLevelsAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsOneLevelAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		public static async Task<List<T>> GetParentsOneLevelAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetParentsOneLevelAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static List<T> GetChildrenOneLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenOneLevelAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		public static async Task<List<T>> GetChildrenOneLevelAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenOneLevelAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static ParentChild<T> GetChildrenTree<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenTreeAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		public static async Task<ParentChild<T>> GetChildrenTreeAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenTreeAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		public static ICollection<IStatement> FindPath<T>(this IEnumerable<IStatement> statements, Type statementType, T parent, T child)
			where T : class
		{
			return TaskHelper.AwaitDetached(() => FindPathAsync<T>(statements, statementType, parent, child));
		}

		public static async Task<ICollection<IStatement>> FindPathAsync<T>(this IEnumerable<IStatement> statements, Type statementType, T parent, T child)
			where T : class
		{
			var typedStatements = await statements.OfType<IParentChild<T>>().Where(statement => statement.GetType() == statementType).ToListAsync();

			// search up (child > parent), because search tree has to be smaller in this case
			var pathsToCheck = await typedStatements.Where(statement => statement.Child == child).Select(statement => new List<IParentChild<T>> { statement }).ToListAsync();
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
						nextStep.AddRange(await typedStatements.Where(statement => statement.Child == lastParent).Select(statement => new List<IParentChild<T>>(path) { statement }).ToListAsync());
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
			return TaskHelper.AwaitDetached(() => GetParentsAllLevelsAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		public static async Task<List<T>> GetParentsAllLevelsAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetRelatedAllLevels(relationships, item, involvedRelationships, GetParentsOneLevelAsync);
		}

		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenAllLevelsAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		public static async Task<List<T>> GetChildrenAllLevelsAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetRelatedAllLevels(relationships, item, involvedRelationships, GetChildrenOneLevelAsync);
		}

		private delegate Task<List<T>> GetRelativesDelegate<T, RelationshipT>(IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null);

		private static async Task<List<T>> GetRelatedAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships, GetRelativesDelegate<T, RelationshipT> getRelatives)
		{
			var result = new List<T>();
			var relativesToCheck = new List<T> { item };
			while (relativesToCheck.Count > 0)
			{
				var nextGeneration = relativesToCheck.Aggregate(new List<T>(), (list, relative) => { list.AddRange(getRelatives(relationships, relative, involvedRelationships).Await()); return list; });
				nextGeneration.RemoveAll(result.Contains);
				relativesToCheck = await nextGeneration.Distinct().ToListAsync();
				result.AddRange(relativesToCheck);
			}
			return result;
		}

		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsOneLevelAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		public static async Task<List<T>> GetParentsOneLevelAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var foundRelationships = await relationships.Where(c => c.Child == item).ToListAsync();
			if (involvedRelationships != null)
			{
				involvedRelationships.AddRange(foundRelationships);
			}
			return await foundRelationships.Select(c => c.Parent).ToListAsync();
		}

		public static List<T> GetChildrenOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenOneLevelAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		public static async Task<List<T>> GetChildrenOneLevelAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var foundRelationships = await relationships.Where(c => c.Parent == item).ToListAsync();
			if (involvedRelationships != null)
			{
				involvedRelationships.AddRange(foundRelationships);
			}
			return await foundRelationships.Select(c => c.Child).ToListAsync();
		}

		/*public static ParentChild<T> GetChildrenTree<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenTreeAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}*/

		public static async Task<ParentChild<T>> GetChildrenTreeAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			var result = new ParentChild<T>(item);

			var itemsToFill = new Queue<ParentChild<T>>();
			itemsToFill.Enqueue(result);

			do
			{
				var currentItem = itemsToFill.Dequeue();
				foreach (var child in await GetChildrenOneLevelAsync(relationships, currentItem.Value, involvedRelationships))
				{
					itemsToFill.Enqueue(new ParentChild<T>(child, currentItem));
				}
			} while (itemsToFill.Count > 0);

			return result;
		}
	}
}
