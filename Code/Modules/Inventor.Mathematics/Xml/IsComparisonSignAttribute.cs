using System.Xml.Serialization;

using Inventor.Core;
using Inventor.Core.Xml;

namespace Inventor.Mathematics.Xml
{
	[XmlType("IsComparisonSign")]
	public class IsComparisonSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsComparisonSignAttribute.Value;
		}
	}
}
