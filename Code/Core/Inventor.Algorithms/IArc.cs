namespace Inventor.Algorithms
{
	public interface IArc<out NodeT>
	{
		NodeT From
		{ get; }

		NodeT To
		{ get; }
	}
}
