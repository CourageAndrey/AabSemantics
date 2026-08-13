namespace AabSemantics.Modules.Boolean.Attributes
{
	/// <summary>Marks a concept as one of the two logical values.</summary>
	public class IsBooleanAttribute : IsValueAttribute
	{
		private IsBooleanAttribute()
		{ }

		/// <summary>The single shared instance; hides <see cref="IsValueAttribute.Value"/>.</summary>
		public new static readonly IsBooleanAttribute Value = new IsBooleanAttribute();
	}
}
