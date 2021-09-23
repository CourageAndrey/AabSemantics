using System.Xml.Serialization;

using Inventor.Core;
using Inventor.Core.Xml;

namespace Inventor.Processes.Xml
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
