using System.Xml.Serialization;

using Inventor.Core;
using Inventor.Core.Xml;

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
