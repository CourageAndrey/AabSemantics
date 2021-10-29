using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public static class Dijkstra
	{
		public static ICollection<SimplePath<NodeT>> FindShortestPaths<NodeT>(this ICollection<IArc<NodeT>> arcs, NodeT fromNode)
			where NodeT : class
		{
			if (arcs == null) throw new ArgumentNullException(nameof(arcs));
			if (fromNode == null) throw new ArgumentNullException(nameof(fromNode));

			var shortestArcs = new Dictionary<NodeT, Dictionary<NodeT, IArc<NodeT>>>();
			foreach (var arc in arcs)
			{
				if (arc.Lenght < 0)
				{
					throw new InvalidOperationException("Arcs with negative length are not allowed.");
				}

				Dictionary<NodeT, IArc<NodeT>> neighboors;
				if (!shortestArcs.TryGetValue(arc.From, out neighboors))
				{
					shortestArcs[arc.From] = neighboors = new Dictionary<NodeT, IArc<NodeT>>();
				}

				IArc<NodeT> neighboor;
				if (!neighboors.TryGetValue(arc.To, out neighboor) || arc.Lenght < neighboor.Lenght)
				{
					neighboors[arc.To] = arc;
				}
			}

			var visitedNodes = new HashSet<NodeT>();
			var currentPath = new SimplePath<NodeT>(fromNode);
			var foundPaths = new Dictionary<NodeT, SimplePath<NodeT>> { { fromNode, currentPath } };

			while (currentPath != null)
			{
				var currentNode = currentPath.To;
				SimplePath<NodeT> nextPath = null;

				Dictionary<NodeT, IArc<NodeT>> currentNeighboors;
				if (shortestArcs.TryGetValue(currentNode, out currentNeighboors))
				{
					foreach (var arc in currentNeighboors.Values)
					{
						var newPath = new SimplePath<NodeT>(currentPath, arc);

						SimplePath<NodeT> oldPath;
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

	public interface IArc<NodeT>
	{
		NodeT From
		{ get; }
		
		NodeT To
		{ get; }

		Double Lenght
		{ get; }
	}

	public interface IPath<NodeT>
	{
		NodeT From
		{ get; }

		NodeT To
		{ get; }

		List<IArc<NodeT>> Path
		{ get; }

		Double Lenght
		{ get; }
	}

	public class SimpleArc<NodeT> : IArc<NodeT>
	{
		#region Properties

		public NodeT From
		{ get; }

		public NodeT To
		{ get; }

		public Double Lenght
		{ get; }

		#endregion

		public SimpleArc(NodeT from, NodeT to, Double lenght)
		{
			From = from;
			To = to;
			Lenght = lenght;
		}
	}

	public class SimplePath<NodeT> : IPath<NodeT>
		where NodeT : class
	{
		#region Properties

		public NodeT From
		{ get; }

		public NodeT To
		{ get; }

		public List<IArc<NodeT>> Path
		{ get; }

		public Double Lenght
		{ get; }

		#endregion

		#region Constructors

		private SimplePath(NodeT from, NodeT to, List<IArc<NodeT>> path, Double lenght)
		{
			From = from;
			To = to;
			Path = path;
			Lenght = lenght;
		}

		public SimplePath(NodeT rootNode)
			: this(rootNode, rootNode, new List<IArc<NodeT>>(), 0)
		{ }

		public SimplePath(SimplePath<NodeT> path, IArc<NodeT> arc)
			: this(path.From, arc.To, new List<IArc<NodeT>>(path.Path) { arc }, path.Lenght + arc.Lenght)
		{
			if (path.To != arc.From)
			{
				throw new InvalidOperationException("To path node has to be the same as arcs From.");
			}
		}

		#endregion
	}
}
