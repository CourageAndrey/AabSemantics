using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>Serializable <see cref="IWpfUiModule"/>, loaded from a language file.</summary>
	[XmlType]
	public class WpfUiModule : LanguageExtension, IWpfUiModule
	{
		#region Xml Properties

		/// <summary>Captions shared by every dialog. In serializable form.</summary>
		[XmlElement(nameof(Common))]
		public LanguageCommon CommonXml
		{ get; set; }

		/// <summary>Wordings of the error dialog. In serializable form.</summary>
		[XmlElement(nameof(Errors))]
		public LanguageErrors ErrorsXml
		{ get; set; }

		/// <summary>Captions of the wizard-style dialogs. In serializable form.</summary>
		[XmlElement(nameof(Ui))]
		public LanguageUi UiXml
		{ get; set; }

		/// <summary>Captions that do not belong to any single dialog. In serializable form.</summary>
		[XmlElement(nameof(Misc))]
		public LanguageMisc MiscXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Captions shared by every dialog.</summary>
		[XmlIgnore]
		public ILanguageCommon Common
		{ get { return CommonXml; } }

		/// <summary>Wordings of the error dialog.</summary>
		[XmlIgnore]
		public ILanguageErrors Errors
		{ get { return ErrorsXml; } }

		/// <summary>Captions of the wizard-style dialogs.</summary>
		[XmlIgnore]
		public ILanguageUi Ui
		{ get { return UiXml; } }

		/// <summary>Captions that do not belong to any single dialog.</summary>
		[XmlIgnore]
		public ILanguageMisc Misc
		{ get { return MiscXml; } }

		#endregion

		/// <summary>Builds the bundle with its built-in English texts.</summary>
		/// <returns>A fully populated bundle.</returns>
		public static WpfUiModule CreateDefault()
		{
			return new WpfUiModule()
			{
				CommonXml = LanguageCommon.CreateDefault(),
				ErrorsXml = LanguageErrors.CreateDefault(),
				UiXml = LanguageUi.CreateDefault(),
				MiscXml = LanguageMisc.CreateDefault(),
			};
		}
	}
}
