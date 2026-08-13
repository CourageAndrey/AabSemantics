using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of the "is a sign" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsSign")]
	public class IsSignAttribute : Attribute<Attributes.IsSignAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsSignAttribute LoadTyped()
		{
			return Attributes.IsSignAttribute.Value;
		}
	}
}
