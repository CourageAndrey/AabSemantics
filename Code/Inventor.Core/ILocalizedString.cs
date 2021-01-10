using System;

namespace Inventor.Core
{
	public interface ILocalizedString
	{
		String GetValue(ILanguage language);
	}
}
