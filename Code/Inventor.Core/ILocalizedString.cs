using System;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public interface ILocalizedString
	{
		String GetValue(ILanguage language);
	}
}
