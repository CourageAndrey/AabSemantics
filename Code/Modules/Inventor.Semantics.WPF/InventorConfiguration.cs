using System;
using System.Xml.Serialization;

namespace Inventor.Semantics.WPF
{
	[Serializable, XmlRoot]
	public class InventorConfiguration
	{
		[XmlElement]
		public String SelectedLanguage
		{ get; set; }

		[XmlIgnore]
		public const string FileName = "Configuration.xml";
	}
}
