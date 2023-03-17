using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
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
