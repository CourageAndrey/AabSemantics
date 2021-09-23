using System.Xml.Serialization;

namespace Inventor.Core.Xml
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
