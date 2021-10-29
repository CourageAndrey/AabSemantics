using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType("IsSign")]
	public class IsSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsSignAttribute.Value;
		}
	}
}
