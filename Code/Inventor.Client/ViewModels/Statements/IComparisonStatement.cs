namespace Inventor.Client.ViewModels.Statements
{
	public interface IComparisonStatement
	{
		ConceptItem LeftValue
		{ get; set; }

		ConceptItem RightValue
		{ get; set; }
	}
}
