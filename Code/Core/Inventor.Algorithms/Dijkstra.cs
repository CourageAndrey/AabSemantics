using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public static class Dijkstra
	{
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

	public interface IArcWithLength<NodeT>
	{
		NodeT From
		{ get; }
		
		NodeT To
		{ get; }

		Double Lenght
		{ get; }
	}

	public interface IPathWithLenght<NodeT>
	{
		NodeT From
		{ get; }

		NodeT To
		{ get; }

		List<IArcWithLength<NodeT>> Path
		{ get; }

		Double Lenght
		{ get; }
	}

	public class SimpleArcWithLength<NodeT> : IArcWithLength<NodeT>
	{
		#region Properties

		public NodeT From
		{ get; }

		public NodeT To
		{ get; }

		public Double Lenght
		{ get; }

		#endregion

		public SimpleArcWithLength(NodeT from, NodeT to, Double lenght)
		{
			From = from;
			To = to;
			Lenght = lenght;
		}
	}

	public class SimplePathWithLenght<NodeT> : IPathWithLenght<NodeT>
		where NodeT : class
	{
		#region Properties

		public NodeT From
		{ get; }

		public NodeT To
		{ get; }

		public List<IArcWithLength<NodeT>> Path
		{ get; }

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

		public SimplePathWithLenght(NodeT rootNode)
			: this(rootNode, rootNode, new List<IArcWithLength<NodeT>>(), 0)
		{ }

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
