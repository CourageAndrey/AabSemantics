using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Mathematics.Xml
{
	[XmlType("IsComparisonSign")]
	public class IsComparisonSignAttribute : Attribute<Attributes.IsComparisonSignAttribute>
	{
		public override Attributes.IsComparisonSignAttribute LoadTyped()
		{
			return Attributes.IsComparisonSignAttribute.Value;
		}
	}
}
