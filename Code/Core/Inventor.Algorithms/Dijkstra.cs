using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	/// <summary>Dijkstra's shortest path algorithm.</summary>
	public static class Dijkstra
	{
		/// <summary>Finds the shortest path from one node to every node reachable from it.</summary>
		/// <typeparam name="NodeT">Node type.</typeparam>
		/// <param name="arcs">Graph arcs; parallel arcs are reduced to the shortest one.</param>
		/// <param name="fromNode">Node to start from.</param>
		/// <returns>One shortest path per reachable node, excluding <paramref name="fromNode"/> itself.</returns>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">An arc has a negative length.</exception>
		public static ICollection<SimplePathWithLenght<NodeT>> FindShortestPaths<NodeT>(this ICollection<IArcWithLength<NodeT>> arcs, NodeT fromNode)
			where NodeT : class
		{
			if (arcs == null) throw new ArgumentNullException(nameof(arcs));
			if (fromNode == null) throw new ArgumentNullException(nameof(fromNode));

			var shortestArcs = new Dictionary<NodeT, Dictionary<NodeT, IArcWithLength<NodeT>>>();
			foreach (var arc in arcs)
			{
				if (arc.Lenght < 0)
				{
					throw new InvalidOperationException("Arcs with negative length are not allowed.");
				}

				Dictionary<NodeT, IArcWithLength<NodeT>> neighboors;
				if (!shortestArcs.TryGetValue(arc.From, out neighboors))
				{
					shortestArcs[arc.From] = neighboors = new Dictionary<NodeT, IArcWithLength<NodeT>>();
				}

				IArcWithLength<NodeT> neighboor;
				if (!neighboors.TryGetValue(arc.To, out neighboor) || arc.Lenght < neighboor.Lenght)
				{
					neighboors[arc.To] = arc;
				}
			}

			var visitedNodes = new HashSet<NodeT>();
			var currentPath = new SimplePathWithLenght<NodeT>(fromNode);
			var foundPaths = new Dictionary<NodeT, SimplePathWithLenght<NodeT>> { { fromNode, currentPath } };

			while (currentPath != null)
			{
				var currentNode = currentPath.To;
				SimplePathWithLenght<NodeT> nextPath = null;

				Dictionary<NodeT, IArcWithLength<NodeT>> currentNeighboors;
				if (shortestArcs.TryGetValue(currentNode, out currentNeighboors))
				{
					foreach (var arc in currentNeighboors.Values)
					{
						var newPath = new SimplePathWithLenght<NodeT>(currentPath, arc);

						SimplePathWithLenght<NodeT> oldPath;
						if (!foundPaths.TryGetValue(arc.To, out oldPath) || newPath.Lenght < oldPath.Lenght)
						{
							foundPaths[arc.To] = newPath;
						}

						if (nextPath == null || nextPath.Lenght > newPath.Lenght)
						{
							nextPath = newPath;
						}
					}
				}

				visitedNodes.Add(currentNode);

				if (nextPath == null)
				{
					nextPath = foundPaths.Values.Where(p => !visitedNodes.Contains(p.To)).OrderBy(p => p.Lenght).FirstOrDefault();
				}
				currentPath = nextPath;
			}

			foundPaths.Remove(fromNode);
			return foundPaths.Values;
		}
	}

	/// <summary>Weighted graph arc.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public interface IArcWithLength<out NodeT> : IArc<NodeT>
	{
		/// <summary>Arc weight; must not be negative.</summary>
		Double Lenght
		{ get; }
	}

	/// <summary>Path through a weighted graph.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public interface IPathWithLenght<NodeT> : IPath<NodeT, IArcWithLength<NodeT>>
	{
		/// <summary>Sum of the weights of the path's arcs.</summary>
		Double Lenght
		{ get; }
	}

	/// <summary>Default <see cref="IArcWithLength{NodeT}"/> implementation.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public class SimpleArcWithLength<NodeT> : IArcWithLength<NodeT>
	{
		#region Properties

		/// <summary>Node the arc starts at.</summary>
		public NodeT From
		{ get; }

		/// <summary>Node the arc leads to.</summary>
		public NodeT To
		{ get; }

		/// <summary>Arc weight.</summary>
		public Double Lenght
		{ get; }

		#endregion

		/// <summary>Creates a weighted arc.</summary>
		/// <param name="from">Node the arc starts at.</param>
		/// <param name="to">Node the arc leads to.</param>
		/// <param name="lenght">Arc weight.</param>
		public SimpleArcWithLength(NodeT from, NodeT to, Double lenght)
		{
			From = from;
			To = to;
			Lenght = lenght;
		}
	}

	/// <summary>Default <see cref="IPathWithLenght{NodeT}"/> implementation; instances are immutable.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public class SimplePathWithLenght<NodeT> : IPathWithLenght<NodeT>
		where NodeT : class
	{
		#region Properties

		/// <summary>Node the path starts at.</summary>
		public NodeT From
		{ get; }

		/// <summary>Node the path leads to.</summary>
		public NodeT To
		{ get; }

		/// <summary>Arcs forming the path, in traversal order.</summary>
		public List<IArcWithLength<NodeT>> Path
		{ get; }

		/// <summary>Sum of the weights of the path's arcs.</summary>
		public Double Lenght
		{ get; }

		#endregion

		#region Constructors

		private SimplePathWithLenght(NodeT from, NodeT to, List<IArcWithLength<NodeT>> path, Double lenght)
		{
			From = from;
			To = to;
			Path = path;
			Lenght = lenght;
		}

		/// <summary>Creates an empty path of zero length, starting and ending at the same node.</summary>
		/// <param name="rootNode">Node the path consists of.</param>
		public SimplePathWithLenght(NodeT rootNode)
			: this(rootNode, rootNode, new List<IArcWithLength<NodeT>>(), 0)
		{ }

		/// <summary>Creates a path extending an existing one by one arc.</summary>
		/// <param name="path">Path to extend; left unchanged.</param>
		/// <param name="arc">Arc to append; must start where the path ends.</param>
		/// <exception cref="InvalidOperationException">The arc does not start at the path's end node.</exception>
		public SimplePathWithLenght(SimplePathWithLenght<NodeT> path, IArcWithLength<NodeT> arc)
			: this(path.From, arc.To, new List<IArcWithLength<NodeT>>(path.Path) { arc }, path.Lenght + arc.Lenght)
		{
			if (path.To != arc.From)
			{
				throw new InvalidOperationException("To path node has to be the same as arcs From.");
			}
		}

		#endregion
	}
}
