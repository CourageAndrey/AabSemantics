using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType("IsProcess")]
	public class IsProcessAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsProcessAttribute.Value;
		}
	}
}
