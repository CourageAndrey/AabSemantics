using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Serialization.Json
{
	/// <summary>JSON surrogate of a localized string: a culture-to-text map.</summary>
	[DataContract]
	public class LocalizedString
	{
		#region Properties

		/// <summary>Text per culture identifier.</summary>
		[DataMember]
		public Dictionary<String, String> Values
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Converts a localized string into its surrogate.</summary>
		/// <param name="source">String to convert; a constant one contributes a single locale.</param>
		/// <exception cref="NotSupportedException">The implementation is neither a variable nor a constant.</exception>
		public LocalizedString(ILocalizedString source)
			: this(source.AsDictionary())
		{ }

		/// <summary>Creates a surrogate over an existing map, used directly rather than copied.</summary>
		/// <param name="values">Text per culture identifier.</param>
		public LocalizedString(Dictionary<String, String> values)
		{
			Values = values;
		}

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public LocalizedString()
			: this(new Dictionary<String, String>())
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
				variable.SetLocale(value.Key, value.Value);
			}
		}
	}
}
