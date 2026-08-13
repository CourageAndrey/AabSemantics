using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// A statement expressing a hierarchical relation between two items, such as "is a" or
	/// "is part of". Implementing this interface is what makes a statement type traversable
	/// by <see cref="ParentChildHelper"/>.
	/// </summary>
	/// <typeparam name="T">Type of the related items, typically <see cref="IConcept"/>.</typeparam>
	public interface IParentChild<out T>
	{
		/// <summary>
		/// The more general side of the relation.
		/// </summary>
		T Parent { get; }

		/// <summary>
		/// The more specific side of the relation.
		/// </summary>
		T Child { get; }
	}

	/// <summary>
	/// A node of a materialized hierarchy tree, as produced by the <c>GetChildrenTree</c> helpers.
	/// Unlike <see cref="IParentChild{T}"/>, which is a single edge, this holds a whole subtree.
	/// </summary>
	/// <typeparam name="T">Type of the items forming the tree.</typeparam>
	public class ParentChild<T>
	{
		/// <summary>
		/// The item this node stands for.
		/// </summary>
		public T Value
		{ get; }

		/// <summary>
		/// Enclosing node, or <c>null</c> at the root of the tree.
		/// </summary>
		public ParentChild<T> Parent
		{ get; private set; }

		/// <summary>
		/// Direct child nodes.
		/// </summary>
		public ICollection<ParentChild<T>> Children
		{ get; }

		/// <summary>
		/// Creates a node and links it to its relatives in both directions.
		/// </summary>
		/// <param name="value">Item the node stands for.</param>
		/// <param name="parent">Enclosing node; the new node adds itself to its children.</param>
		/// <param name="children">Child nodes; each has its parent set to the new node.</param>
		public ParentChild(T value, ParentChild<T> parent = null, IEnumerable<ParentChild<T>> children = null)
		{
			Value = value;

			SetParent(parent);

			Children = new List<ParentChild<T>>();
			SetChildren(children);
		}

		/// <summary>
		/// Re-parents the node, adding it to the new parent's children.
		/// The previous parent, if any, is not updated.
		/// </summary>
		/// <param name="parent">New enclosing node, or <c>null</c> to detach.</param>
		public void SetParent(ParentChild<T> parent)
		{
			Parent = parent;

			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}

		/// <summary>
		/// Replaces the node's children, pointing each new child back at this node.
		/// </summary>
		/// <param name="children">New child nodes; <c>null</c> just clears the existing ones.</param>
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

	/// <summary>
	/// Traverses hierarchies built from <see cref="IParentChild{T}"/> statements.
	/// <para>
	/// Every method comes in four shapes: one level or all levels, parents or children, and each
	/// of those synchronously or asynchronously. Overloads accepting <see cref="IStatement"/>
	/// sequences filter them down to the requested relationship type first.
	/// </para>
	/// <para>
	/// The optional <c>involvedRelationships</c> argument is an output accumulator: when supplied,
	/// every relationship the traversal walks through is appended to it. Question processors pass
	/// a list here to collect the statements that make up an answer's explanation.
	/// </para>
	/// </summary>
	public static class ParentChildHelper
	{
		/// <summary>
		/// Returns every ancestor of <paramref name="item"/> — its parents, their parents, and so on.
		/// The traversal is cycle-safe and never returns duplicates.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the ancestors of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All ancestors, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsAllLevelsAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns every ancestor of <paramref name="item"/>, at all levels.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the ancestors of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All ancestors, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static async Task<List<T>> GetParentsAllLevelsAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetParentsAllLevelsAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		/// <summary>
		/// Returns every descendant of <paramref name="item"/> — its children, their children, and so on.
		/// The traversal is cycle-safe and never returns duplicates.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the descendants of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All descendants, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenAllLevelsAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns every descendant of <paramref name="item"/>, at all levels.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the descendants of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All descendants, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static async Task<List<T>> GetChildrenAllLevelsAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenAllLevelsAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		/// <summary>
		/// Returns the direct parents of <paramref name="item"/>, without recursing further.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the parents of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate parents.</returns>
		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsOneLevelAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns the direct parents of <paramref name="item"/>.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the parents of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate parents.</returns>
		public static async Task<List<T>> GetParentsOneLevelAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetParentsOneLevelAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		/// <summary>
		/// Returns the direct children of <paramref name="item"/>, without recursing further.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the children of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate children.</returns>
		public static List<T> GetChildrenOneLevel<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenOneLevelAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns the direct children of <paramref name="item"/>.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to find the children of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate children.</returns>
		public static async Task<List<T>> GetChildrenOneLevelAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenOneLevelAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		/// <summary>
		/// Builds the full subtree rooted at <paramref name="item"/>, preserving its shape
		/// rather than flattening it the way <c>GetChildrenAllLevels</c> does.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to use as the tree's root.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The root node, with descendants attached.</returns>
		public static ParentChild<T> GetChildrenTree<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenTreeAsync<T, RelationshipT>(statements, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously builds the full subtree rooted at <paramref name="item"/>.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="statements">Statements to search; those of other types are ignored.</param>
		/// <param name="item">Item to use as the tree's root.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The root node, with descendants attached.</returns>
		public static async Task<ParentChild<T>> GetChildrenTreeAsync<T, RelationshipT>(this IEnumerable<IStatement> statements, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetChildrenTreeAsync(statements.OfType<RelationshipT>(), item, involvedRelationships);
		}

		/// <summary>
		/// Finds a chain of relationships connecting <paramref name="parent"/> down to
		/// <paramref name="child"/>, which is what proves that the two are related at all.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <param name="statements">Statements to search.</param>
		/// <param name="statementType">Exact relationship type to follow; subtypes are not matched.</param>
		/// <param name="parent">Item the path starts from.</param>
		/// <param name="child">Item the path leads to.</param>
		/// <returns>The connecting statements, or an empty collection when no path exists.</returns>
		public static ICollection<IStatement> FindPath<T>(this IEnumerable<IStatement> statements, Type statementType, T parent, T child)
			where T : class
		{
			return TaskHelper.AwaitDetached(() => FindPathAsync<T>(statements, statementType, parent, child));
		}

		/// <summary>
		/// Asynchronously finds a chain of relationships connecting <paramref name="parent"/>
		/// down to <paramref name="child"/>. Searches upwards from the child, because that
		/// side of the hierarchy is normally the narrower one.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <param name="statements">Statements to search.</param>
		/// <param name="statementType">Exact relationship type to follow; subtypes are not matched.</param>
		/// <param name="parent">Item the path starts from.</param>
		/// <param name="child">Item the path leads to.</param>
		/// <returns>The connecting statements, or an empty collection when no path exists.</returns>
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

		/// <summary>
		/// Returns every ancestor of <paramref name="item"/>, at all levels, from an already
		/// filtered sequence of relationships.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the ancestors of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All ancestors, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static List<T> GetParentsAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsAllLevelsAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns every ancestor of <paramref name="item"/>, at all levels.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the ancestors of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All ancestors, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static async Task<List<T>> GetParentsAllLevelsAsync<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return await GetRelatedAllLevels(relationships, item, involvedRelationships, GetParentsOneLevelAsync);
		}

		/// <summary>
		/// Returns every descendant of <paramref name="item"/>, at all levels, from an already
		/// filtered sequence of relationships.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the descendants of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All descendants, nearest generation first. <paramref name="item"/> itself is not included.</returns>
		public static List<T> GetChildrenAllLevels<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenAllLevelsAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns every descendant of <paramref name="item"/>, at all levels.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the descendants of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>All descendants, nearest generation first. <paramref name="item"/> itself is not included.</returns>
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

		/// <summary>
		/// Returns the direct parents of <paramref name="item"/> from an already filtered
		/// sequence of relationships.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the parents of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate parents.</returns>
		public static List<T> GetParentsOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetParentsOneLevelAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns the direct parents of <paramref name="item"/>: the parent side
		/// of every relationship whose child is that item.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the parents of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate parents.</returns>
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

		/// <summary>
		/// Returns the direct children of <paramref name="item"/> from an already filtered
		/// sequence of relationships.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the children of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate children.</returns>
		public static List<T> GetChildrenOneLevel<T, RelationshipT>(this IEnumerable<RelationshipT> relationships, T item, List<RelationshipT> involvedRelationships = null)
			where RelationshipT : IParentChild<T>
			where T : class
		{
			return TaskHelper.AwaitDetached(() => GetChildrenOneLevelAsync<T, RelationshipT>(relationships, item, involvedRelationships));
		}

		/// <summary>
		/// Asynchronously returns the direct children of <paramref name="item"/>: the child side
		/// of every relationship whose parent is that item.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to find the children of.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The immediate children.</returns>
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

		/// <summary>
		/// Asynchronously builds the full subtree rooted at <paramref name="item"/>, expanding it
		/// breadth-first. Cyclic relationships would make this recurse forever, so the data is
		/// expected to be acyclic.
		/// </summary>
		/// <typeparam name="T">Type of the related items.</typeparam>
		/// <typeparam name="RelationshipT">Relationship statement type to follow.</typeparam>
		/// <param name="relationships">Relationships to traverse.</param>
		/// <param name="item">Item to use as the tree's root.</param>
		/// <param name="involvedRelationships">Optional accumulator receiving every relationship traversed.</param>
		/// <returns>The root node, with descendants attached.</returns>
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
