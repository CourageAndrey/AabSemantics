namespace Inventor.Core.Attributes
{
	public class IsValueAttribute : IAttribute
	{
		private IsValueAttribute()
		{ }

		public static readonly IsValueAttribute Value = new IsValueAttribute();
	}
}
