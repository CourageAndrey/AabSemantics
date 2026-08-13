using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
{
	/// <summary>XML surrogate of the "is sequence sign" attribute; carries no data beyond its element name.</summary>
	[XmlType("IsSequenceSign")]
	public class IsSequenceSignAttribute : Attribute<Attributes.IsSequenceSignAttribute>
	{
		/// <summary>Returns the shared attribute instance.</summary>
		/// <returns>The attribute.</returns>
		public override Attributes.IsSequenceSignAttribute LoadTyped()
		{
			return Attributes.IsSequenceSignAttribute.Value;
		}
	}
}
