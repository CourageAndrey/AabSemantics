using System.Xml.Serialization;

using Inventor.Semantics;

namespace Samples.Semantics.Sample07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAttribute : Inventor.Semantics.Xml.Attribute
	{
		public override IAttribute Load()
		{
			return Sample07.CustomModule.CustomAttribute.Value;
		}
	}
}
