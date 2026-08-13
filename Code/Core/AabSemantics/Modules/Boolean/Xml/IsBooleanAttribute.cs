using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
{
	/// <summary>XML surrogate of the "is a logical value" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsBoolean")]
	public class IsBooleanAttribute : Attribute<Attributes.IsBooleanAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsBooleanAttribute LoadTyped()
		{
			return Attributes.IsBooleanAttribute.Value;
		}
	}
}
