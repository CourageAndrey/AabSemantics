namespace AabSemantics.Modules.Mathematics.Attributes
{
	/// <summary>Marks a concept as one of the comparison signs.</summary>
	public class IsComparisonSignAttribute : IAttribute
	{
		private IsComparisonSignAttribute()
		{ }

		/// <summary>The single shared instance of the attribute.</summary>
		public static readonly IsComparisonSignAttribute Value = new IsComparisonSignAttribute();
	}
}
