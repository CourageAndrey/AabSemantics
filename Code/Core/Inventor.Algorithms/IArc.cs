namespace Inventor.Algorithms
{
	/// <summary>Directed edge of a graph.</summary>
	/// <typeparam name="NodeT">Node type.</typeparam>
	public interface IArc<out NodeT>
	{
		/// <summary>Node the arc starts at.</summary>
		NodeT From
		{ get; }

		/// <summary>Node the arc leads to.</summary>
		NodeT To
		{ get; }
	}
}
