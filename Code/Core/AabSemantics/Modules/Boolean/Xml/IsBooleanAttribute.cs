using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
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
