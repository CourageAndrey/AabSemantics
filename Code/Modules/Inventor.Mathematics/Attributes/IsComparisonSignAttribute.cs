using Inventor.Semantics;

namespace Inventor.Mathematics.Attributes
{
	public class IsComparisonSignAttribute : IAttribute
	{
		private IsComparisonSignAttribute()
		{ }

		public static readonly IsComparisonSignAttribute Value = new IsComparisonSignAttribute();
	}
}
