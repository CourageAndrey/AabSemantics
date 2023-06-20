using System.Xml.Serialization;

using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
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
