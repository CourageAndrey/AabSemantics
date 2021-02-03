using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public abstract class LocalizedString
	{
		public abstract void Apply(ILocalizedString localizedString);

		public abstract Core.Localization.LocalizedStringVariable Create();

		public static LocalizedString From(ILocalizedString value)
		{
			return value is LocalizedStringVariable
				? new LocalizedStringVariable(value as Core.Localization.LocalizedStringVariable) as LocalizedString
				: new LocalizedStringConstant(value as Core.Localization.LocalizedStringConstant);
		}
	}

	public class LocalizedStringVariable : LocalizedString
	{
		public List<LocalizedStringValue> Values
		{ get; }

		public LocalizedStringVariable()
			: this(new Dictionary<string, string>())
		{ }

		public LocalizedStringVariable(Core.Localization.LocalizedStringVariable localizedString)
			: this(localizedString.Locales.ToDictionary(locale => locale, localizedString.GetValue))
		{ }

		public LocalizedStringVariable(IDictionary<string, string> locales)
		{
			Values = locales.Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList();
		}

		public override void Apply(ILocalizedString localizedString)
		{
			var variableString = localizedString as Core.Localization.LocalizedStringVariable;
			if (variableString != null)
			{
				variableString.Clear();
				foreach (var value in Values)
				{
					variableString.SetLocale(value.Locale, value.Value);
				}
			}
		}

		public override Core.Localization.LocalizedStringVariable Create()
		{
			return new Core.Localization.LocalizedStringVariable(Values.ToDictionary(
				value => value.Locale,
				value => value.Value));
		}
	}

	public class LocalizedStringConstant : LocalizedString
	{
		public Core.Localization.LocalizedStringConstant Original
		{ get; }

		public LocalizedStringConstant(Core.Localization.LocalizedStringConstant original)
		{
			Original = original;
		}

		public override void Apply(ILocalizedString localizedString)
		{ }

		public override Core.Localization.LocalizedStringVariable Create()
		{
			throw new NotSupportedException();
		}
	}
}
