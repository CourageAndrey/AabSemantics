using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics;

namespace Inventor.WPF.ViewModels
{
	public abstract class LocalizedString
	{
		public abstract void Apply(ILocalizedString localizedString);

		public abstract Semantics.Localization.LocalizedStringVariable Create();

		public static LocalizedString From(ILocalizedString value)
		{
			return value is Semantics.Localization.LocalizedStringVariable
				? new LocalizedStringVariable(value as Semantics.Localization.LocalizedStringVariable) as LocalizedString
				: new LocalizedStringConstant(value as Semantics.Localization.LocalizedStringConstant);
		}
	}

	public class LocalizedStringVariable : LocalizedString
	{
		public List<LocalizedStringValue> Values
		{ get; }

		public LocalizedStringVariable()
			: this(new Dictionary<string, string>())
		{ }

		public LocalizedStringVariable(Semantics.Localization.LocalizedStringVariable localizedString)
			: this(localizedString.Locales.ToDictionary(locale => locale, localizedString.GetValue))
		{ }

		public LocalizedStringVariable(IDictionary<string, string> locales)
		{
			Values = locales.Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList();
		}

		public override void Apply(ILocalizedString localizedString)
		{
			var variableString = localizedString as Semantics.Localization.LocalizedStringVariable;
			if (variableString != null)
			{
				variableString.Clear();
				foreach (var value in Values)
				{
					variableString.SetLocale(value.Locale, value.Value);
				}
			}
		}

		public override Semantics.Localization.LocalizedStringVariable Create()
		{
			return new Semantics.Localization.LocalizedStringVariable(Values.ToDictionary(
				value => value.Locale,
				value => value.Value));
		}
	}

	public class LocalizedStringConstant : LocalizedString
	{
		public Semantics.Localization.LocalizedStringConstant Original
		{ get; }

		public LocalizedStringConstant(Semantics.Localization.LocalizedStringConstant original)
		{
			Original = original;
		}

		public override void Apply(ILocalizedString localizedString)
		{ }

		public override Semantics.Localization.LocalizedStringVariable Create()
		{
			throw new NotSupportedException();
		}
	}
}
