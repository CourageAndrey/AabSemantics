using System.Xml.Serialization;

using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Processes.Xml
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
