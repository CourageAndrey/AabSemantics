using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
{
	/// <summary>XML surrogate of the "is process" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsProcess")]
	public class IsProcessAttribute : Attribute<Attributes.IsProcessAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsProcessAttribute LoadTyped()
		{
			return Attributes.IsProcessAttribute.Value;
		}
	}
}
