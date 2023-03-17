using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Processes.Xml
{
	[XmlType("IsSequenceSign")]
	public class IsSequenceSignAttribute : Attribute<Attributes.IsSequenceSignAttribute>
	{
		public override Attributes.IsSequenceSignAttribute LoadTyped()
		{
			return Attributes.IsSequenceSignAttribute.Value;
		}
	}
}
