using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class IsComparisonSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsComparisonSignAttribute.Value;
		}
	}
}
