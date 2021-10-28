using System.Xml.Serialization;

using Inventor.Semantics;
using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Processes.Xml
{
	[XmlType("IsProcess")]
	public class IsProcessAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Processes.Attributes.IsProcessAttribute.Value;
		}
	}
}
