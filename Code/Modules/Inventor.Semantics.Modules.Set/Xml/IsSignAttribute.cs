using System.Xml.Serialization;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Set.Xml
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
