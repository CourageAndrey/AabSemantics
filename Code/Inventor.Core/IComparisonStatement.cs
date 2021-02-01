namespace Inventor.Core
{
	public interface IComparisonStatement
	{
		IConcept LeftValue
		{ get; }

		IConcept RightValue
		{ get; }
	}
}
