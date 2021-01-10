using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Localization
{
	public abstract class LocalizedString : ILocalizedString
	{
		public abstract String GetValue(ILanguage language);

		public abstract override String ToString();

		public static readonly LocalizedString Empty = new LocalizedStringConstant(language => String.Empty);

		public static implicit operator LocalizedString(Func<ILanguage, String> getter)
		{
			return new LocalizedStringConstant(getter);
		}
	}

	public sealed class LocalizedStringVariable : LocalizedString
	{
		#region Properties

		private readonly IDictionary<String, String> _values = new SortedDictionary<String, String>();

		public override String GetValue(ILanguage language)
		{
			if (_values.Count == 0) return null;

			String result, locale = language.Culture;
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

		public LocalizedStringVariable(String culture, String text)
		{
			_values[culture] = text;
		}

		public LocalizedStringVariable(IEnumerable<KeyValuePair<String, String>> values)
			: this()
		{
			foreach (var pair in values)
			{
				_values[pair.Key] = pair.Value;
			}
		}

		#endregion

		public override String ToString()
		{
			String result = String.Format("{0} ({1} values)", Strings.TostringLocalized, _values.Count);
			if (_values.Count > 0)
			{
				result += String.Format(" ([0] = \"{0}\")", _values.Values.First());
			}
			return result;
		}

		#region Editing

		public void SetLocale(String locale, String value)
		{
			_values[locale] = value;
		}

		public void RemoveLocale(String locale)
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

		private readonly Func<ILanguage, String> _getter;

		public override String GetValue(ILanguage language)
		{
			return _getter(language);
		}

		#endregion

		public LocalizedStringConstant(Func<ILanguage, String> getter)
		{
			_getter = getter;
		}

		public override String ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringLocalized, _getter(Language.Default));
		}
	}
}
