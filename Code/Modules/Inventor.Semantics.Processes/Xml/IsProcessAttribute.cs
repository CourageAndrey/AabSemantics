using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Processes.Xml
{
	[XmlType("IsProcess")]
	public class IsProcessAttribute : Attribute<Attributes.IsProcessAttribute>
	{
		public override Attributes.IsProcessAttribute LoadTyped()
		{
			return Attributes.IsProcessAttribute.Value;
		}
	}
}
