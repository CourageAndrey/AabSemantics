using System;

namespace Inventor.Semantics.Localization
{
	public class AbsentLocaleException : Exception
	{
		#region Properties

		public String Locale
		{ get; }

		#endregion

		#region Constructors

		public AbsentLocaleException(String locale)
		{
			Locale = locale;
		}

		#endregion
	}
}
