namespace Inventor.Semantics.WPF.ViewModels
{
	public class LocalizedStringValue
	{
		public string Locale
		{ get; set; }

		public string Value
		{ get; set; }

		public LocalizedStringValue(string locale, string value)
		{
			Locale = locale;
			Value = value;
		}

		public LocalizedStringValue()
			: this(null, null)
		{ }
	}
}
