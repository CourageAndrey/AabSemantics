using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Xml
{
	[XmlType]
	public class LocalizedString
	{
		#region Properties

		[XmlArray(nameof(Values))]
		[XmlArrayItem("Value")]
		public List<LocalizedStringValue> Values
		{ get; set; }

		#endregion

		#region Constructors

		public LocalizedString(ILocalizedString source)
			: this()
		{
			var variable = (LocalizedStringVariable) source;
			foreach (String locale in variable.Locales)
			{
				Values.Add(new LocalizedStringValue(locale, variable.GetValue(locale)));
			}
		}

		public LocalizedString()
		{
			Values = new List<LocalizedStringValue>();
		}

		#endregion

		public void LoadTo(ILocalizedString destination)
		{
			var variable = (LocalizedStringVariable) destination;
			variable.Clear();

			foreach (var value in Values)
			{
				variable.SetLocale(value.Locale, value.Value);
			}
		}
	}

	[XmlType]
	public class LocalizedStringValue
	{
		#region Properties

		[XmlAttribute]
		public String Locale
		{ get; set; }

		[XmlAttribute]
		public String Value
		{ get; set; }

		#endregion

		#region Constructors

		public LocalizedStringValue()
			: this(null, null)
		{ }

		public LocalizedStringValue(String locale, String value)
		{
			Locale = locale;
			Value = value;
		}

		#endregion
	}
}
