using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Json
{
	[Serializable]
	public class LocalizedString
	{
		#region Properties

		public Dictionary<String, String> Values
		{ get; set; }

		#endregion

		#region Constructors

		public LocalizedString(ILocalizedString source)
			: this()
		{
#warning Get rid of such dangerous typecast!
			var variable = (LocalizedStringVariable) source;
			Values = variable.Locales.ToDictionary(
				locale => locale,
				locale => variable.GetValue(locale));
		}

		public LocalizedString(Dictionary<String, String> values)
		{
			Values = values;
		}

		public LocalizedString()
			: this(new Dictionary<String, String>())
		{ }

		#endregion

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
