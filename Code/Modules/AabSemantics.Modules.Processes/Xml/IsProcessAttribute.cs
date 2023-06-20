using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
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
