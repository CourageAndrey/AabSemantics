using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Modules.Boolean.Xml
{
	[XmlType("IsValue")]
	public class IsValueAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsValueAttribute.Value;
		}
	}
}
