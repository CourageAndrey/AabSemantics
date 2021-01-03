using System;
using System.Xml.Serialization;

namespace Inventor.Core
{
	[Serializable, XmlRoot]
	public class InventorConfiguration
	{
		[XmlElement]
		public String SelectedLanguage
		{ get; set; }

		[XmlIgnore]
		internal const string FileName = "Configuration.xml";
	}
}
