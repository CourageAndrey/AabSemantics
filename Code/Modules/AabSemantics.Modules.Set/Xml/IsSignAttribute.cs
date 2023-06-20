using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType("IsSign")]
	public class IsSignAttribute : Attribute<Attributes.IsSignAttribute>
	{
		public override Attributes.IsSignAttribute LoadTyped()
		{
			return Attributes.IsSignAttribute.Value;
		}
	}
}
