using System.Collections.Generic;
using System.Linq;

using Inventor.Core;
using Inventor.Core.Localization;

namespace Inventor.Client.ViewModels
{
	public class LocalizedString
	{
		public List<LocalizedStringValue> Values
		{ get; }

		public LocalizedString()
			: this(new Dictionary<string, string>())
		{ }

		public LocalizedString(Core.Localization.LocalizedStringVariable localizedString)
			: this(localizedString.Locales.ToDictionary(locale => locale, localizedString.GetValue))
		{ }

		public LocalizedString(IDictionary<string, string> locales)
		{
			Values = locales.Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList();
		}

		public void Apply(ILocalizedString localizedString)
		{
			var variableString = localizedString as LocalizedStringVariable;
			if (variableString != null)
			{
				variableString.Clear();
				foreach (var value in Values)
				{
					variableString.SetLocale(value.Locale, value.Value);
				}
			}
		}

		public LocalizedStringVariable Create()
		{
			return new LocalizedStringVariable(Values.ToDictionary(
				value => value.Locale,
				value => value.Value));
		}

		public static LocalizedString From(ILocalizedString value)
		{
			return value is LocalizedStringVariable ? new LocalizedString(value as LocalizedStringVariable) : null;
		}
	}
}
