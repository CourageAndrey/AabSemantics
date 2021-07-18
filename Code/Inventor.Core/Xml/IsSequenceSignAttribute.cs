using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class IsSequenceSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsSequenceSignAttribute.Value;
		}
	}
}
