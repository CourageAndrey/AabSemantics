using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>XML surrogate of a localized string: a flat list of locale-and-text pairs.</summary>
	[XmlType]
	public class LocalizedString
	{
		#region Properties

		/// <summary>The per-locale texts.</summary>
		[XmlArray(nameof(Values))]
		[XmlArrayItem("Value")]
		public List<LocalizedStringValue> Values
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Converts a localized string into its surrogate.</summary>
		/// <param name="source">String to convert; a constant one contributes a single locale.</param>
		/// <exception cref="NotSupportedException">The implementation is neither a variable nor a constant.</exception>
		public LocalizedString(ILocalizedString source)
			: this(source.AsDictionary().Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList())
		{ }

		/// <summary>Creates a surrogate over an existing list, used directly rather than copied.</summary>
		/// <param name="values">The per-locale texts.</param>
		public LocalizedString(List<LocalizedStringValue> values)
		{
			Values = values;
		}

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public LocalizedString()
			: this(new List<LocalizedStringValue>())
		{ }

		#endregion

		/// <summary>Replaces the destination's locales with the ones held here.</summary>
		/// <param name="destination">String to fill; must be an editable per-locale string.</param>
		/// <exception cref="InvalidCastException">The destination is not a <see cref="LocalizedStringVariable"/>.</exception>
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

	/// <summary>One locale-and-text pair of a serialized localized string.</summary>
	[XmlType]
	public class LocalizedStringValue
	{
		#region Properties

		/// <summary>Culture identifier, e.g. <c>en-US</c>.</summary>
		[XmlAttribute]
		public String Locale
		{ get; set; }

		/// <summary>Text for that culture.</summary>
		[XmlAttribute]
		public String Value
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty pair, as required by the XML serializer.</summary>
		public LocalizedStringValue()
			: this(null, null)
		{ }

		/// <summary>Creates a locale-and-text pair.</summary>
		/// <param name="locale">Culture identifier.</param>
		/// <param name="value">Text for that culture.</param>
		public LocalizedStringValue(String locale, String value)
		{
			Locale = locale;
			Value = value;
		}

		#endregion
	}
}
