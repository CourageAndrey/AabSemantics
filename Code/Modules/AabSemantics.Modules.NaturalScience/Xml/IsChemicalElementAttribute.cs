using System;
using System.Xml.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.NaturalScience.Xml
{
	[XmlType("IsChemicalElement")]
	public class IsChemicalElementAttribute : Attribute<Attributes.IsChemicalElementAttribute>
	{
		#region Properties

		[XmlAttribute]
		public Byte AtomicNumber
		{ get; set; }

		[XmlAttribute]
		public String Symbol
		{ get; set; }

		[XmlAttribute]
		public String Name
		{ get; set; }

		[XmlAttribute]
		public Byte Group
		{ get; set; }

		[XmlAttribute]
		public Byte Period
		{ get; set; }

		[XmlAttribute]
		public Attributes.Block Block
		{ get; set; }

		#endregion

		public override Attributes.IsChemicalElementAttribute LoadTyped()
		{
			throw new NotSupportedException();
		}
	}
}
