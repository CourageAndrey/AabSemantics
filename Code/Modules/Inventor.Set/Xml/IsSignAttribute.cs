using System.Xml.Serialization;

using Inventor.Core;
using Inventor.Core.Xml;

namespace Inventor.Set.Xml
{
	[XmlType("IsSign")]
	public class IsSignAttribute : Attribute
	{
		public override IAttribute Load()
		{
			return Attributes.IsSignAttribute.Value;
		}
	}
}
