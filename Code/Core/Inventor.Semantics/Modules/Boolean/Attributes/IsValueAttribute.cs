namespace Inventor.Semantics.Modules.Boolean.Attributes
{
	public class IsValueAttribute : IAttribute
	{
		protected IsValueAttribute()
		{ }

		public static readonly IsValueAttribute Value = new IsValueAttribute();
	}
}
