using Inventor.Semantics.WPF.Localization;

namespace Inventor.Semantics.WPF
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
