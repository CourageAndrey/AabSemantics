namespace Inventor.Semantics.Attributes
{
	public class IsValueAttribute : IAttribute
	{
		protected IsValueAttribute()
		{ }

		public static readonly IsValueAttribute Value = new IsValueAttribute();
	}
}
