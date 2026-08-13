namespace AabSemantics.Modules.Boolean.Attributes
{
	/// <summary>Marks a concept as a value, i.e. something other concepts can be equal to.</summary>
	public class IsValueAttribute : IAttribute
	{
		/// <summary>Prevents outside instantiation; use <see cref="Value"/>.</summary>
		protected IsValueAttribute()
		{ }

		/// <summary>The single shared instance of the attribute.</summary>
		public static readonly IsValueAttribute Value = new IsValueAttribute();
	}
}
