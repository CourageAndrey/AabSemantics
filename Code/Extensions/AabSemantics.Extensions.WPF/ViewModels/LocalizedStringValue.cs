namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Editable locale-and-text pair shown as one row of the localized string editor.</summary>
	public class LocalizedStringValue
	{
		/// <summary>Culture identifier.</summary>
		public string Locale
		{ get; set; }

		/// <summary>The value concept.</summary>
		public string Value
		{ get; set; }

		/// <summary>Creates a row.</summary>
		/// <param name="locale">Culture identifier.</param>
		/// <param name="value">Text for that culture.</param>
		public LocalizedStringValue(string locale, string value)
		{
			Locale = locale;
			Value = value;
		}

		/// <summary>Creates an empty row.</summary>
		public LocalizedStringValue()
			: this(null, null)
		{ }
	}
}
