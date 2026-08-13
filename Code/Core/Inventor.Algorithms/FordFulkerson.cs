using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Algorithms
{
	/// <summary>Ford-Fulkerson maximum flow algorithm.</summary>
	public static class FordFulkerson
	{
		/// <summary>Computes the maximum flow between two given nodes.</summary>
		/// <typeparam name="NodeT">Node type.</typeparam>
		/// <param name="arcs">Graph arcs with their capacities; the collection is copied, not modified.</param>
		/// <param name="sourceNode">Node the flow originates at.</param>
		/// <param name="destinationNode">Node the flow drains into.</param>
		/// <returns>The maximum flow value, or zero when no path exists.</returns>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
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

		/// <summary>Computes the maximum flow, inferring source and destination from the graph shape.</summary>
		/// <typeparam name="NodeT">Node type.</typeparam>
		/// <param name="arcs">Graph arcs with their capacities.</param>
		/// <returns>The maximum flow value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="arcs"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The graph does not have exactly one node without inputs and one without outputs.</exception>
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

	/// <summary>Graph arc with a flow capacity.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public interface IArcWithFlow<out NodeT> : IArc<NodeT>
	{
		/// <summary>Capacity of the arc.</summary>
		Double Flow
		{ get; }
	}

	/// <summary>Default <see cref="IArcWithFlow{NodeT}"/> implementation.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public class SimpleArcWithFlow<NodeT> : IArcWithFlow<NodeT>
	{
		#region Properties

		/// <summary>Node the arc starts at.</summary>
		public NodeT From
		{ get; }

		/// <summary>Node the arc leads to.</summary>
		public NodeT To
		{ get; }

		/// <summary>Capacity of the arc.</summary>
		public Double Flow
		{ get; }

		#endregion

		/// <summary>Creates an arc with a capacity.</summary>
		/// <param name="from">Node the arc starts at.</param>
		/// <param name="to">Node the arc leads to.</param>
		/// <param name="flow">Capacity of the arc.</param>
		public SimpleArcWithFlow(NodeT from, NodeT to, Double flow)
		{
			From = from;
			To = to;
			Flow = flow;
		}

		/// <summary>Creates an arc connecting the same nodes as an existing one, with a different capacity.</summary>
		/// <param name="arc">Arc to copy the endpoints from.</param>
		/// <param name="flow">Capacity of the new arc.</param>
		public SimpleArcWithFlow(IArcWithFlow<NodeT> arc, Double flow)
		{
			From = arc.From;
			To = arc.To;
			Flow = flow;
		}
	}
}
