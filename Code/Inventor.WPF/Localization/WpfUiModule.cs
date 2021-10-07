using System.Xml.Serialization;

using Inventor.Core.Localization;

namespace Inventor.WPF.Localization
{
	[XmlType]
	public class WpfUiModule : LanguageExtension, IWpfUiModule
	{
		#region Xml Properties

		[XmlElement(nameof(Common))]
		public LanguageCommon CommonXml
		{ get; set; }

		[XmlElement(nameof(Errors))]
		public LanguageErrors ErrorsXml
		{ get; set; }

		[XmlElement(nameof(Ui))]
		public LanguageUi UiXml
		{ get; set; }

		[XmlElement(nameof(Misc))]
		public LanguageMisc MiscXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageCommon Common
		{ get { return CommonXml; } }

		[XmlIgnore]
		public ILanguageErrors Errors
		{ get { return ErrorsXml; } }

		[XmlIgnore]
		public ILanguageUi Ui
		{ get { return UiXml; } }

		[XmlIgnore]
		public ILanguageMisc Misc
		{ get { return MiscXml; } }

		#endregion

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
