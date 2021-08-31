namespace Inventor.Core.Attributes
{
	public class IsBooleanAttribute : IsValueAttribute
	{
		private IsBooleanAttribute()
		{ }

		public new static readonly IsBooleanAttribute Value = new IsBooleanAttribute();
	}
}
