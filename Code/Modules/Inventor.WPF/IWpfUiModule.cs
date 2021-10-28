using Inventor.WPF.Localization;

namespace Inventor.WPF
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
