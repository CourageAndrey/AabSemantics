using System.Xml.Serialization;

using Inventor.Core;

namespace Samples._07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAttribute : Inventor.Core.Xml.Attribute
	{
		public override IAttribute Load()
		{
			return _07.CustomModule.CustomAttribute.Value;
		}
	}
}
