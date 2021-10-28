using System.Xml.Serialization;

namespace Inventor.Semantics.Xml
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
