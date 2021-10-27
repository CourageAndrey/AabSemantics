using System.Xml.Serialization;

using Inventor.Semantics;
using Inventor.Semantics.Xml;

namespace Inventor.Processes.Xml
{
	[XmlType("IsSequenceSign")]
	public class IsSequenceSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsSequenceSignAttribute.Value;
		}
	}
}
