using System;

namespace Inventor.Core.Localization
{
	public class AbsentLocaleException : Exception
	{
		#region Properties

		public string Locale
		{ get { return _locale; } }

		private readonly string _locale;

		#endregion

		#region Constructors

		public AbsentLocaleException(string locale)
		{
			_locale = locale;
		}

		#endregion
	}
}
