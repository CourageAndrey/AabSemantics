using System;

namespace AabSemantics.Localization
{
	/// <summary>Thrown when a localized string has no value for the requested locale.</summary>
	public class AbsentLocaleException : Exception
	{
		#region Properties

		/// <summary>Culture identifier that has no value.</summary>
		public String Locale
		{ get; }

		#endregion

		#region Constructors

		/// <summary>Creates the exception.</summary>
		/// <param name="locale">Culture identifier that has no value.</param>
		public AbsentLocaleException(String locale)
		{
			Locale = locale;
		}

		#endregion
	}
}
