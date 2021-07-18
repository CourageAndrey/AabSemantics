using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class IsValueAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsValueAttribute.Value;
		}
	}
}
