using System.Collections.Generic;

namespace Inventor.Algorithms
{
	public interface IPath<out NodeT, ArcT>
		where ArcT : IArc<NodeT>
	{
		NodeT From
		{ get; }

		NodeT To
		{ get; }

		List<ArcT> Path
		{ get; }
	}
}
