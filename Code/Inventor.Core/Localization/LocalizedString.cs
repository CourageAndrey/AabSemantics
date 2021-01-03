using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Localization
{
	public abstract class LocalizedString
	{
		public abstract string GetValue(ILanguage language);

		public abstract override string ToString();
	}

	public sealed class LocalizedStringVariable : LocalizedString
	{
		#region Properties

		private readonly IDictionary<string, string> _values = new SortedDictionary<string, string>();

		public override string GetValue(ILanguage language)
		{
			if (_values.Count == 0) return null;

			string result, locale = language.Culture;
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

		public LocalizedStringVariable()
		{ }

		public LocalizedStringVariable(string culture, string text)
		{
			_values[culture] = text;
		}

		public LocalizedStringVariable(IEnumerable<KeyValuePair<string, string>> values)
			: this()
		{
			foreach (var pair in values)
			{
				_values[pair.Key] = pair.Value;
			}
		}

		#endregion

		public override string ToString()
		{
			string result = string.Format("{0} ({1} values)", Strings.TostringLocalized, _values.Count);
			if (_values.Count > 0)
			{
				result += string.Format(" ([0] = \"{0}\")", _values.Values.First());
			}
			return result;
		}

		#region Editing

		public void SetLocale(string locale, string value)
		{
			_values[locale] = value;
		}

		public void RemoveLocale(string locale)
		{
			_values.Remove(locale);
		}

		public void Clear()
		{
			_values.Clear();
		}

		#endregion
	}

	public sealed class LocalizedStringConstant : LocalizedString
	{
		#region Properties

		private readonly Func<ILanguage, string> _getter;

		public override string GetValue(ILanguage language)
		{
			return _getter(language);
		}

		#endregion

		public LocalizedStringConstant(Func<ILanguage, string> getter)
		{
			_getter = getter;
		}

		public override string ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringLocalized, _getter(Language.Current));
		}
	}
}
