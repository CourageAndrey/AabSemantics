using System.Xml.Serialization;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Processes.Xml
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
