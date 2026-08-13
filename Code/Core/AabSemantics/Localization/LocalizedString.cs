using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AabSemantics.Localization
{
	/// <summary>
	/// Base of the two localized-string flavours: a constant that derives its text from the
	/// language, and a variable that stores one text per locale.
	/// </summary>
	public abstract class LocalizedString : ILocalizedString
	{
		/// <summary>Resolves the string for a language.</summary>
		/// <param name="language">Language to render in.</param>
		/// <returns>The text in that language.</returns>
		public abstract String GetValue(ILanguage language);

		/// <summary>Formats the string for diagnostics.</summary>
		/// <returns>Diagnostic string.</returns>
		public abstract override String ToString();

		/// <summary>A string that is empty in every language.</summary>
		public static readonly LocalizedString Empty = new LocalizedStringConstant(language => String.Empty);

		/// <summary>Wraps a language-to-text function as a constant localized string.</summary>
		/// <param name="getter">Derives the text from a language.</param>
		/// <returns>A constant localized string.</returns>
		public static implicit operator LocalizedString(Func<ILanguage, String> getter)
		{
			return new LocalizedStringConstant(getter);
		}
	}

	/// <summary>
	/// A localized string holding an explicit text per locale, as edited by the user and
	/// persisted with the knowledge base.
	/// </summary>
	public class LocalizedStringVariable : LocalizedString
	{
		#region Properties

		/// <summary>Culture identifiers that have a text, in sorted order.</summary>
		public ICollection<String> Locales
		{ get { return _values.Keys; } }

		private readonly IDictionary<String, String> _values = new SortedDictionary<String, String>();

		/// <summary>Resolves the string for a language, by its culture.</summary>
		/// <param name="language">Language to render in.</param>
		/// <returns>The stored text, or <c>null</c> when no locale has been set at all.</returns>
		/// <exception cref="AbsentLocaleException">Texts exist, but not for this language's culture.</exception>
		public override String GetValue(ILanguage language)
		{
			return GetValue(language.Culture);
		}

		/// <summary>Resolves the string for a culture identifier.</summary>
		/// <param name="locale">Culture identifier to look up.</param>
		/// <returns>
		/// The stored text, or <c>null</c> when the string is entirely empty — note that an empty
		/// string yields <c>null</c> rather than throwing.
		/// </returns>
		/// <exception cref="AbsentLocaleException">Texts exist, but not for this locale.</exception>
		public String GetValue(String locale)
		{
			if (_values.Count == 0) return null;

			String result;
			if (_values.TryGetValue(locale, out result))
			{
				return result;
			}
			else
			{
				throw new AbsentLocaleException(locale);
			}
		}

		#endregion

		#region Constructors

		/// <summary>Creates a string with no locales set.</summary>
		public LocalizedStringVariable()
		{ }

		/// <summary>Creates a string with a single locale set.</summary>
		/// <param name="culture">Culture identifier.</param>
		/// <param name="text">Text for that culture.</param>
		public LocalizedStringVariable(String culture, String text)
		{
			_values[culture] = text;
		}

		/// <summary>Creates a string from culture-to-text pairs.</summary>
		/// <param name="values">Pairs to store; later entries win on duplicate cultures.</param>
		public LocalizedStringVariable(IEnumerable<KeyValuePair<String, String>> values)
			: this()
		{
			foreach (var pair in values)
			{
				_values[pair.Key] = pair.Value;
			}
		}

		#endregion

		/// <summary>Formats the string as a locale count plus the first stored text.</summary>
		/// <returns>Diagnostic string.</returns>
		public override String ToString()
		{
			String result = String.Format(CultureInfo.InvariantCulture, "{0} ({1} values)", Strings.TostringLocalized, _values.Count);
			if (_values.Count > 0)
			{
				result += String.Format(CultureInfo.InvariantCulture, " ([0] = \"{0}\")", _values.Values.First());
			}
			return result;
		}

		#region Editing

		/// <summary>Sets or replaces the text for a locale.</summary>
		/// <param name="locale">Culture identifier.</param>
		/// <param name="value">Text for that culture.</param>
		public void SetLocale(String locale, String value)
		{
			_values[locale] = value;
		}

		/// <summary>Removes the text for a locale, if present.</summary>
		/// <param name="locale">Culture identifier.</param>
		public void RemoveLocale(String locale)
		{
			_values.Remove(locale);
		}

		/// <summary>Removes every locale, making the string empty again.</summary>
		public void Clear()
		{
			_values.Clear();
		}

		#endregion
	}

	/// <summary>
	/// A localized string that computes its text from the language rather than storing it —
	/// used for built-in wordings, which live in the language files instead of the knowledge base.
	/// </summary>
	public class LocalizedStringConstant : LocalizedString
	{
		#region Properties

		private readonly Func<ILanguage, String> _getter;

		/// <summary>Resolves the string for a language.</summary>
		/// <param name="language">Language to render in.</param>
		/// <returns>The text in that language.</returns>
		public override String GetValue(ILanguage language)
		{
			return _getter(language);
		}

		#endregion

		/// <summary>Creates a constant localized string.</summary>
		/// <param name="getter">Derives the text from a language; not validated against <c>null</c>.</param>
		public LocalizedStringConstant(Func<ILanguage, String> getter)
		{
			_getter = getter;
		}

		/// <summary>Formats the string as its text in the default language.</summary>
		/// <returns>Diagnostic string.</returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} \"{1}\"", Strings.TostringLocalized, _getter(Language.Default));
		}
	}
}
