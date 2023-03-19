using System.Xml.Serialization;

using Inventor.Semantics.Serialization;

namespace Samples.Semantics.Sample07.CustomModule.Xml
{
	[XmlType("Custom")]
	public class CustomStatement : Inventor.Semantics.Serialization.Xml.Statement
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
		{
			ID = statement.ID;
			Concept1 = statement.Concept1?.ID;
			Concept2 = statement.Concept2?.ID;
		}

		#endregion

		public override Inventor.Semantics.IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Sample07.CustomModule.CustomStatement(ID, conceptIdResolver.GetConceptById(Concept1), conceptIdResolver.GetConceptById(Concept2));
		}
	}
}
