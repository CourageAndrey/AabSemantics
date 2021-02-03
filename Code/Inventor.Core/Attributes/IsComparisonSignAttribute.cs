namespace Inventor.Core.Attributes
{
	public class IsComparisonSignAttribute : IAttribute
	{
		private IsComparisonSignAttribute()
		{ }

		public static readonly IsComparisonSignAttribute Value = new IsComparisonSignAttribute();
	}
}
