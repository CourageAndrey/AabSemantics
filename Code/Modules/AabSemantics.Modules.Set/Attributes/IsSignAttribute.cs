namespace AabSemantics.Modules.Set.Attributes
{
	/// <summary>Marks a concept as a sign, i.e. a property other concepts can have values for.</summary>
	public class IsSignAttribute : IAttribute
	{
		private IsSignAttribute()
		{ }

		/// <summary>The single shared instance of the attribute.</summary>
		public static readonly IsSignAttribute Value = new IsSignAttribute();
	}
}
