using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	public static class FordFulkerson
	{
		public static Double FindMaximumFlow<NodeT>(this ICollection<IArcWithFlow<NodeT>> arcs, NodeT sourceNode, NodeT destinationNode)
			where NodeT : class
		{
			if (arcs == null) throw new ArgumentNullException(nameof(arcs));
			if (sourceNode == null) throw new ArgumentNullException(nameof(sourceNode));
			if (destinationNode == null) throw new ArgumentNullException(nameof(destinationNode));

			arcs = new List<IArcWithFlow<NodeT>>(arcs);
			Double maxFlow = 0;

			var path = getPath(arcs, sourceNode, destinationNode);
			while (path != null)
			{
				Double flow = path.Min(arc => arc.Flow);
				maxFlow += flow;

				foreach (var arc in path)
				{
					arcs.Remove(arc);

					Double arcFlow = arc.Flow - flow;
					if (arcFlow > Double.Epsilon)
					{
						arcs.Add(new SimpleArcWithFlow<NodeT>(arc, arcFlow));
					}
				}

				path = getPath(arcs, sourceNode, destinationNode);
			}

			return maxFlow;
		}

		private static List<IArcWithFlow<NodeT>> getPath<NodeT>(ICollection<IArcWithFlow<NodeT>> arcs, NodeT sourceNode,  NodeT destinationNode)
			where NodeT : class
		{
			var pathsToCheck = arcs.Where(arc => arc.From == sourceNode && arc.To != sourceNode).Select(arc => new List<IArcWithFlow<NodeT>> { arc }).ToList();

			var visitedNodes = new HashSet<NodeT>(pathsToCheck.Select(path => path[0].To)) { sourceNode };

			while (pathsToCheck.Count > 0)
			{
				foreach (var path in pathsToCheck)
				{
					if (path[path.Count - 1].To == destinationNode)
					{
						return path;
					}
				}

				var nextStepPaths = new List<List<IArcWithFlow<NodeT>>>();
				foreach (var path in pathsToCheck)
				{
					var lastNode = path[path.Count - 1].To;
					foreach (var arc in arcs.Where(a => a.From == lastNode && !visitedNodes.Contains(a.To)))
					{
						nextStepPaths.Add(new List<IArcWithFlow<NodeT>>(path) { arc });
					}
				}
				pathsToCheck = nextStepPaths;
			}

			return null;
		}

		public static Double FindMaximumFlow<NodeT>(this ICollection<IArcWithFlow<NodeT>> arcs)
			where NodeT : class
		{
			if (arcs == null) throw new ArgumentNullException(nameof(arcs));

			var sourceNodes = new HashSet<NodeT>();
			var destinationNodes = new HashSet<NodeT>();
			foreach (var arc in arcs)
			{
				sourceNodes.Add(arc.From);
				destinationNodes.Add(arc.To);
			}

			NodeT sourceNode = null;
			foreach (var node in sourceNodes)
			{
				if (!destinationNodes.Remove(node))
				{
					if (sourceNode == null)
					{
						sourceNode = node;
					}
					else
					{
						throw new ArgumentException("Only one source node (without inputs) allowed.");
					}
				}
			}
			if (sourceNode == null)
			{
				throw new ArgumentException("No source node (without inputs) found.");
			}

			if (destinationNodes.Count == 1)
			{
				var destinationNode = destinationNodes.First();
				return FindMaximumFlow(arcs, sourceNode, destinationNode);
			}
			else if (destinationNodes.Count > 1)
			{
				throw new ArgumentException("Only one destination node (without outputs) allowed.");
			}
			else
			{
				throw new ArgumentException("No destination node (without outputs) found.");
			}
		}
	}

	public interface IArcWithFlow<out NodeT> : IArc<NodeT>
	{
		Double Flow
		{ get; }
	}

	public class SimpleArcWithFlow<NodeT> : IArcWithFlow<NodeT>
	{
		#region Properties

		public NodeT From
		{ get; }

		public NodeT To
		{ get; }

		public Double Flow
		{ get; }

		#endregion

		public SimpleArcWithFlow(NodeT from, NodeT to, Double flow)
		{
			From = from;
			To = to;
			Flow = flow;
		}

		public SimpleArcWithFlow(IArcWithFlow<NodeT> arc, Double flow)
		{
			From = arc.From;
			To = arc.To;
			Flow = flow;
		}
	}
}
