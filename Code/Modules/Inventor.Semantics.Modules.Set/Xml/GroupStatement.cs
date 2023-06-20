using System;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Set.Xml
{
	[XmlType("Group")]
	public class GroupStatement : Statement<Statements.GroupStatement>
	{
		#region Properties

		[XmlAttribute]
		public String Area
		{ get; set; }

		[XmlAttribute]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
		{ }

		public GroupStatement(Set.Statements.GroupStatement statement)
		{
			ID = statement.ID;
			Area = statement.Area?.ID;
			Concept = statement.Concept?.ID;
		}

		#endregion

		protected override Statements.GroupStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(ID, conceptIdResolver.GetConceptById(Area), conceptIdResolver.GetConceptById(Concept));
		}
	}
}
