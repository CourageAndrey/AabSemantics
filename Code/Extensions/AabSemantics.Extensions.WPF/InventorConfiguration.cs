using System;
using System.Xml.Serialization;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>Persisted user settings of the application.</summary>
	[Serializable, XmlRoot]
	public class InventorConfiguration
	{
		/// <summary>Culture identifier of the language last chosen by the user.</summary>
		[XmlElement]
		public String SelectedLanguage
		{ get; set; }

		/// <summary>File name the settings are stored under.</summary>
		[XmlIgnore]
		public const string FileName = "Configuration.xml";
	}
}
