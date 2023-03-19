using System.Xml.Serialization;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Boolean.Xml
{
	[XmlType("IsBoolean")]
	public class IsBooleanAttribute : Attribute<Attributes.IsBooleanAttribute>
	{
		public override Attributes.IsBooleanAttribute LoadTyped()
		{
			return Attributes.IsBooleanAttribute.Value;
		}
	}
}
