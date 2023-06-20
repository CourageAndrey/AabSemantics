using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
{
	[XmlType("IsValue")]
	public class IsValueAttribute : Attribute<Attributes.IsValueAttribute>
	{
		public override Attributes.IsValueAttribute LoadTyped()
		{
			return Attributes.IsValueAttribute.Value;
		}
	}
}
