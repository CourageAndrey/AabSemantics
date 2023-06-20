using System.Xml.Serialization;

namespace AabSemantics.Sample07.CustomModule.Xml
{
	[XmlType("IsCustom")]
	public class CustomAttribute : AabSemantics.Serialization.Xml.Attribute<Sample07.CustomModule.CustomAttribute>
	{
		public override Sample07.CustomModule.CustomAttribute LoadTyped()
		{
			return Sample07.CustomModule.CustomAttribute.Value;
		}
	}
}
