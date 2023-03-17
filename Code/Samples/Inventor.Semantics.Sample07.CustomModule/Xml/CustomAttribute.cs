using System.Xml.Serialization;

namespace Samples.Semantics.Sample07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAttribute : Inventor.Semantics.Xml.Attribute<Sample07.CustomModule.CustomAttribute>
	{
		public override Sample07.CustomModule.CustomAttribute LoadTyped()
		{
			return Sample07.CustomModule.CustomAttribute.Value;
		}
	}
}
