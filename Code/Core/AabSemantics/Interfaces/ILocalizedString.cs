using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// A string whose text depends on the language it is read in. Implementations are either
	/// constants shared by every language or per-locale variables edited by the user.
	/// </summary>
	public interface ILocalizedString
	{
		/// <summary>
		/// Resolves the string for a language.
		/// </summary>
		/// <param name="language">Language to render in.</param>
		/// <returns>The text in that language.</returns>
		String GetValue(ILanguage language);
	}

	/// <summary>
	/// Helpers for persisting localized strings.
	/// </summary>
	public static class LocalizedStringExtensions
	{
		/// <summary>
		/// Flattens a localized string into a culture-to-text map suitable for serialization.
		/// </summary>
		/// <param name="localizedString">String to flatten; must not be <c>null</c>.</param>
		/// <returns>
		/// Every defined locale for a variable string; a single entry under the default
		/// language's culture for a constant one.
		/// </returns>
		/// <exception cref="NotSupportedException">The implementation is neither a variable nor a constant.</exception>
		public static Dictionary<String, String> AsDictionary(this ILocalizedString localizedString)
		{
			localizedString.EnsureNotNull(nameof(localizedString));

			if (localizedString is LocalizedStringVariable variable)
			{
				return variable.Locales.ToDictionary(
					locale => locale,
					locale => variable.GetValue(locale));
			}
			else if (localizedString is LocalizedStringConstant constant)
			{
				var language = Language.Default;
				return new Dictionary<String, String>
				{
					{ language.Culture, constant.GetValue(language) },
				};
			}
			else
			{
				throw new NotSupportedException(localizedString.GetType().FullName);
			}
		}
	}
}
