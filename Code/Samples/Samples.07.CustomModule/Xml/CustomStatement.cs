using System.Xml.Serialization;

namespace Samples._07.CustomModule.Xml
{
	[XmlType("Custom")]
	public class CustomStatement : Inventor.Core.Xml.Statement
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

		public CustomStatement(_07.CustomModule.CustomStatement statement)
		{
			ID = statement.ID;
			Concept1 = statement.Concept1?.ID;
			Concept2 = statement.Concept2?.ID;
		}

		#endregion

		public override Inventor.Core.IStatement Save(Inventor.Core.Xml.ConceptIdResolver conceptIdResolver)
		{
			return new _07.CustomModule.CustomStatement(ID, conceptIdResolver.GetConceptById(Concept1), conceptIdResolver.GetConceptById(Concept2));
		}
	}
}
