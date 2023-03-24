using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics
{
	public interface ILocalizedString
	{
		String GetValue(ILanguage language);
	}

	public static class LocalizedStringExtensions
	{
		public static Dictionary<String, String> AsDictionary(this ILocalizedString localizedString)
		{
			if (localizedString == null)
			{
				throw new ArgumentNullException(nameof(localizedString));
			}
			else if (localizedString is LocalizedStringVariable variable)
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
