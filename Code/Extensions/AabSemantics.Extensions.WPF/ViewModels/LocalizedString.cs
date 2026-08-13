using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Editable view over a localized string. Constants are read-only, variables carry one text per locale.</summary>
	public abstract class LocalizedString
	{
		/// <summary>Writes the edited texts onto an existing localized string.</summary>
		/// <param name="localizedString">String to update.</param>
		public abstract void Apply(ILocalizedString localizedString);

		/// <summary>Builds a new localized string from the edited texts.</summary>
		/// <returns>The created string.</returns>
		public abstract AabSemantics.Localization.LocalizedStringVariable Create();

		/// <summary>Wraps a localized string in the matching view model.</summary>
		/// <param name="value">String to wrap.</param>
		/// <returns>A variable or constant view model, depending on the argument's kind.</returns>
		public static LocalizedString From(ILocalizedString value)
		{
			return value is AabSemantics.Localization.LocalizedStringVariable
				? new LocalizedStringVariable(value as AabSemantics.Localization.LocalizedStringVariable) as LocalizedString
				: new LocalizedStringConstant(value as AabSemantics.Localization.LocalizedStringConstant);
		}
	}

	/// <summary>Editable view over a per-locale localized string.</summary>
	public class LocalizedStringVariable : LocalizedString
	{
		/// <summary>The per-locale texts being edited.</summary>
		public List<LocalizedStringValue> Values
		{ get; }

		/// <summary>Creates an empty view model.</summary>
		public LocalizedStringVariable()
			: this(new Dictionary<string, string>())
		{ }

		/// <summary>Creates a view model over an existing string's locales.</summary>
		/// <param name="localizedString">String to edit.</param>
		public LocalizedStringVariable(AabSemantics.Localization.LocalizedStringVariable localizedString)
			: this(localizedString.Locales.ToDictionary(locale => locale, localizedString.GetValue))
		{ }

		/// <summary>Creates a view model from culture-to-text pairs.</summary>
		/// <param name="locales">Texts per culture identifier.</param>
		public LocalizedStringVariable(IDictionary<string, string> locales)
		{
			Values = locales.Select(locale => new LocalizedStringValue(locale.Key, locale.Value)).ToList();
		}

		/// <summary>Replaces the target's locales with the edited ones.</summary>
		/// <param name="localizedString">String to update.</param>
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

		/// <summary>Builds a new per-locale string from the edited texts.</summary>
		/// <returns>The created string.</returns>
		public override AabSemantics.Localization.LocalizedStringVariable Create()
		{
			return new AabSemantics.Localization.LocalizedStringVariable(Values.ToDictionary(
				value => value.Locale,
				value => value.Value));
		}
	}

	/// <summary>Read-only view over a computed localized string; editing it has no effect.</summary>
	public class LocalizedStringConstant : LocalizedString
	{
		/// <summary>The wrapped constant string.</summary>
		public AabSemantics.Localization.LocalizedStringConstant Original
		{ get; }

		/// <summary>Wraps a constant string.</summary>
		/// <param name="original">String to wrap.</param>
		public LocalizedStringConstant(AabSemantics.Localization.LocalizedStringConstant original)
		{
			Original = original;
		}

		/// <summary>Replaces the target's locales with the edited ones.</summary>
		/// <param name="localizedString">String to update.</param>
		public override void Apply(ILocalizedString localizedString)
		{ }

		/// <summary>Builds a new per-locale string from the edited texts.</summary>
		/// <returns>The created string.</returns>
		public override AabSemantics.Localization.LocalizedStringVariable Create()
		{
			throw new NotSupportedException();
		}
	}
}
