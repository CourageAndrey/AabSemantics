using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	public abstract class LocalizedString
	{
		public abstract void Apply(ILocalizedString localizedString);

		public abstract AabSemantics.Localization.LocalizedStringVariable Create();

		public static LocalizedString From(ILocalizedString value)
		{
			return value is AabSemantics.Localization.LocalizedStringVariable
				? new LocalizedStringVariable(value as AabSemantics.Localization.LocalizedStringVariable) as LocalizedString
				: new LocalizedStringConstant(value as AabSemantics.Localization.LocalizedStringConstant);
		}
	}

	public class LocalizedStringVariable : LocalizedString
	{
		public List<LocalizedStringValue> Values
		{ get; }

		public LocalizedStringVariable()
			: this(new Dictionary<string, string>())
		{ }

		public LocalizedStringVariable(AabSemantics.Localization.LocalizedStringVariable localizedString)
			: this(localizedString.Locales.ToDictionary(locale => locale, localizedString.GetValue))
		{ }

		public LocalizedStringVariable(IDictionary<string, string> locales)
		{
			Values = locales.Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList();
		}

		public override void Apply(ILocalizedString localizedString)
		{
			var variableString = localizedString as AabSemantics.Localization.LocalizedStringVariable;
			if (variableString != null)
			{
				variableString.Clear();
				foreach (var value in Values)
				{
					variableString.SetLocale(value.Locale, value.Value);
				}
			}
		}

		public override AabSemantics.Localization.LocalizedStringVariable Create()
		{
			return new AabSemantics.Localization.LocalizedStringVariable(Values.ToDictionary(
				value => value.Locale,
				value => value.Value));
		}
	}

	public class LocalizedStringConstant : LocalizedString
	{
		public AabSemantics.Localization.LocalizedStringConstant Original
		{ get; }

		public LocalizedStringConstant(AabSemantics.Localization.LocalizedStringConstant original)
		{
			Original = original;
		}

		public override void Apply(ILocalizedString localizedString)
		{ }

		public override AabSemantics.Localization.LocalizedStringVariable Create()
		{
			throw new NotSupportedException();
		}
	}
}
