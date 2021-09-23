using Inventor.Core;

namespace Inventor.Processes.Attributes
{
	public class IsSequenceSignAttribute : IAttribute
	{
		private IsSequenceSignAttribute()
		{ }

		public static readonly IsSequenceSignAttribute Value = new IsSequenceSignAttribute();
	}
}
