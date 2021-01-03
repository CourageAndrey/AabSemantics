using System;
using System.Xml.Serialization;

using Sef.Interfaces;

namespace Inventor.Core
{
	[Serializable, XmlRoot]
	public class InventorConfiguration : IEditable<InventorConfiguration>
	{
		[XmlElement]
		public String SelectedLanguage
		{ get; set; }

		[XmlIgnore]
		internal const string FileName = "Configuration.xml";

		public void UpdateFrom(InventorConfiguration other)
		{
			SelectedLanguage = other.SelectedLanguage;
		}
	}
}
