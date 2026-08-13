using AabSemantics.Extensions.WPF.Localization;
using AabSemantics.Localization;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>The WPF UI module's string bundle.</summary>
	public interface IWpfUiModule : ILanguageExtension
	{
		/// <summary>Captions shared by every dialog.</summary>
		ILanguageCommon Common
		{ get; }

		/// <summary>Wordings of the error dialog.</summary>
		ILanguageErrors Errors
		{ get; }

		/// <summary>Captions of the wizard-style dialogs.</summary>
		ILanguageUi Ui
		{ get; }

		/// <summary>Captions that do not belong to any single dialog.</summary>
		ILanguageMisc Misc
		{ get; }
	}
}
