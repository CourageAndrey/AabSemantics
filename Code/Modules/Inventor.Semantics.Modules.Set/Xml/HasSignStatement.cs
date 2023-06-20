using System;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType("HasSign")]
	public class HasSignStatement : Statement<Statements.HasSignStatement>
	{
		#region Properties

		[XmlAttribute]
		public String Concept
		{ get; set; }

		[XmlAttribute]
		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement()
		{ }

		public HasSignStatement(Set.Statements.HasSignStatement statement)
		{
			ID = statement.ID;
			Concept = statement.Concept?.ID;
			Sign = statement.Sign?.ID;
		}

		#endregion

		protected override Statements.HasSignStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasSignStatement(ID, conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign));
		}
	}
}
