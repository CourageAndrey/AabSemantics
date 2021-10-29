using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Mathematics.Xml
{
	[XmlType("IsComparisonSign")]
	public class IsComparisonSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsComparisonSignAttribute.Value;
		}
	}
}
