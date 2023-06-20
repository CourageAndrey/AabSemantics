using System;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Set.Xml
{
	[XmlType("HasPart")]
	public class HasPartStatement : Statement<Statements.HasPartStatement>
	{
		#region Properties

		[XmlAttribute]
		public String Whole
		{ get; set; }

		[XmlAttribute]
		public String Part
		{ get; set; }

		#endregion

		#region Constructors

		public HasPartStatement()
		{ }

		public HasPartStatement(Set.Statements.HasPartStatement statement)
		{
			ID = statement.ID;
			Whole = statement.Whole?.ID;
			Part = statement.Part?.ID;
		}

		#endregion

		protected override Statements.HasPartStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(ID, conceptIdResolver.GetConceptById(Whole), conceptIdResolver.GetConceptById(Part));
		}
	}
}
