using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Test
{
	internal static class TestHelper
	{
		#region Concept creation

		public static IConcept CreateConcept()
		{
			return string.Empty.CreateConcept();
		}

		public static IConcept CreateConcept(this int number)
		{
			return number.ToString().CreateConcept();
		}

		public static IConcept CreateConcept(this string name)
		{
			return new Concept(new LocalizedStringConstant(language => name));
		}

		#endregion
	}
}
