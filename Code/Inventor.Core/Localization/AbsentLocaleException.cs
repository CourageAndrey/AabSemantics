using System;

namespace Inventor.Core.Localization
{
	public class AbsentLocaleException : Exception
	{
		#region Properties

		public string Locale
		{ get { return locale; } }

		private readonly string locale;

		#endregion

		#region Constructors

		public AbsentLocaleException(string locale)
		{
			this.locale = locale;
		}

		#endregion
	}
}
