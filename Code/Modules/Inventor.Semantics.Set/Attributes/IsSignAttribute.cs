using Inventor.Semantics;

namespace Inventor.Semantics.Set.Attributes
{
	public class IsSignAttribute : IAttribute
	{
		private IsSignAttribute()
		{ }

		public static readonly IsSignAttribute Value = new IsSignAttribute();
	}
}
