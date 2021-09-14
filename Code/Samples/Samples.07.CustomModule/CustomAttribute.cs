using Inventor.Core;

namespace Samples._07.CustomModule
{
	internal class CustomAttribute : IAttribute
	{
		protected CustomAttribute()
		{ }

		public static readonly CustomAttribute Value = new CustomAttribute();
	}
}
