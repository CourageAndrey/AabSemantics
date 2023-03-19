using System.Xml.Serialization;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Boolean.Xml
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
