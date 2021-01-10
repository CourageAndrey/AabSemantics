using System;

namespace Inventor.Core.Localization
{
	public class AbsentLocaleException : Exception
	{
		#region Properties

		public String Locale
		{ get { return _locale; } }

		private readonly String _locale;

		#endregion

		#region Constructors

		public AbsentLocaleException(String locale)
		{
			_locale = locale;
		}

		#endregion
	}
}
