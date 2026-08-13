using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Mathematics.Xml
{
	/// <summary>XML surrogate of the "is a comparison sign" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsComparisonSign")]
	public class IsComparisonSignAttribute : Attribute<Attributes.IsComparisonSignAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsComparisonSignAttribute LoadTyped()
		{
			return Attributes.IsComparisonSignAttribute.Value;
		}
	}
}
