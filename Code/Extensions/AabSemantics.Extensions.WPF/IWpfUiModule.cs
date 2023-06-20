using AabSemantics.Extensions.WPF.Localization;

namespace AabSemantics.Extensions.WPF
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
