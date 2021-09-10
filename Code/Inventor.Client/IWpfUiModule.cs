using Inventor.Client.Localization;

namespace Inventor.Client
{
	public interface IWpfUiModule
	{
		ILanguageCommon Common
		{ get; }

		ILanguageErrors Errors
		{ get; }

		ILanguageUi Ui
		{ get; }

		ILanguageMisc Misc
		{ get; }
	}
}
