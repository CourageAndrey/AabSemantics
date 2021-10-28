using System.Xml.Serialization;

using Inventor.Semantics;

namespace Samples._07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAttribute : Inventor.Semantics.Xml.Attribute
	{
		public override IAttribute Load()
		{
			return _07.CustomModule.CustomAttribute.Value;
		}
	}
}
