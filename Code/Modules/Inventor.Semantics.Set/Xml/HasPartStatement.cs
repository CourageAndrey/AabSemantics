using System;
using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType("HasPart")]
	public class HasPartStatement : Statement
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

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(ID, conceptIdResolver.GetConceptById(Whole), conceptIdResolver.GetConceptById(Part));
		}
	}
}
