using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Modules.Boolean.Xml
{
	[XmlType("IsBoolean")]
	public class IsBooleanAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsBooleanAttribute.Value;
		}
	}
}
