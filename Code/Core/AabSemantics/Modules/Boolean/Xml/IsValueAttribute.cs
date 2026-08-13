using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
{
	/// <summary>XML surrogate of the "is a value" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsValue")]
	public class IsValueAttribute : Attribute<Attributes.IsValueAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsValueAttribute LoadTyped()
		{
			return Attributes.IsValueAttribute.Value;
		}
	}
}
