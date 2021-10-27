using System;

namespace Inventor.Semantics
{
	public interface ILocalizedString
	{
		String GetValue(ILanguage language);
	}
}
