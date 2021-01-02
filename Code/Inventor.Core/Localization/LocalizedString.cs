using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Localization
{
	public abstract class LocalizedString
	{
		public abstract string Value
		{ get; }

		public abstract override string ToString();
	}

	public sealed class LocalizedStringVariable : LocalizedString
	{
		#region Properties

		private readonly IDictionary<string, string> values = new SortedDictionary<string, string>();

		public override string Value
		{
			get
			{
				if (values.Count == 0) return null;

				string result, locale = LanguageEx.CurrentEx.Culture;
				if (values.TryGetValue(locale, out result))
				{
					return result;
				}
				else
				{
					throw new AbsentLocaleException(locale);
				}
			}
		}

		#endregion

		#region Constructors

		public LocalizedStringVariable()
		{ }

		public LocalizedStringVariable(string culture, string text)
		{
			values[culture] = text;
		}

		public LocalizedStringVariable(IEnumerable<KeyValuePair<string, string>> values)
			: this()
		{
			foreach (var pair in values)
			{
				this.values[pair.Key] = pair.Value;
			}
		}

		#endregion

		public override string ToString()
		{
			string result = string.Format("{0} ({1} values)", Strings.TostringLocalized, values.Count);
			if (values.Count > 0)
			{
				result += string.Format(" ([0] = \"{0}\")", values.Values.First());
			}
			return result;
		}

		#region Editing

		public void SetLocale(string locale, string value)
		{
			values[locale] = value;
		}

		public void RemoveLocale(string locale)
		{
			values.Remove(locale);
		}

		public void Clear()
		{
			values.Clear();
		}

		#endregion
	}

	public sealed class LocalizedStringConstant : LocalizedString
	{
		#region Properties

		private readonly Func<string> getter;

		public override string Value
		{ get { return getter(); } }

		#endregion

		public LocalizedStringConstant(Func<string> getter)
		{
			this.getter = getter;
		}

		public override string ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringLocalized, getter());
		}
	}
}
