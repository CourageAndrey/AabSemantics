using System.Xml.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Sample07.CustomModule.Xml
{
	[XmlType("Custom")]
	public class CustomStatement : Serialization.Xml.Statement<Sample07.CustomModule.CustomStatement>
	{
		#region Properties

		[XmlAttribute]
		public string Concept1
		{ get; set; }

		[XmlAttribute]
		public string Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
		{ }

		public CustomStatement(Sample07.CustomModule.CustomStatement statement)
			: base(statement)
		{
			Concept1 = statement.Concept1?.ID;
			Concept2 = statement.Concept2?.ID;
		}

		#endregion

		protected override Sample07.CustomModule.CustomStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Sample07.CustomModule.CustomStatement(ID, conceptIdResolver.GetConceptById(Concept1), conceptIdResolver.GetConceptById(Concept2));
		}
	}
}
