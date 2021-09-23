namespace Inventor.Core.Attributes
{
	public class IsProcessAttribute : IAttribute
	{
		private IsProcessAttribute()
		{ }

		public static readonly IsProcessAttribute Value = new IsProcessAttribute();
	}
}
