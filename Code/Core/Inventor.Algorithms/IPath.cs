using System.Collections.Generic;

namespace Inventor.Algorithms
{
	/// <summary>Chain of arcs connecting two nodes.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	/// <typeparam name="ArcT">Arc type.</typeparam>
	public interface IPath<out NodeT, ArcT>
		where ArcT : IArc<NodeT>
	{
		/// <summary>Node the path starts at.</summary>
		NodeT From
		{ get; }

		/// <summary>Node the path leads to.</summary>
		NodeT To
		{ get; }

		/// <summary>Arcs forming the path, in traversal order.</summary>
		List<ArcT> Path
		{ get; }
	}
}
