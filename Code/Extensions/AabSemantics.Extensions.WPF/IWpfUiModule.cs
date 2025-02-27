using AabSemantics.Extensions.WPF.Localization;
using AabSemantics.Localization;

namespace AabSemantics.Extensions.WPF
{
	public interface IWpfUiModule : ILanguageExtension
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
