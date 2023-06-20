using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Mathematics.Xml
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
