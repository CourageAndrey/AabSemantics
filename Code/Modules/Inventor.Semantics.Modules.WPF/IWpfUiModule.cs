using Inventor.Semantics.Modules.WPF.Localization;

namespace Inventor.Semantics.Modules.WPF
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
