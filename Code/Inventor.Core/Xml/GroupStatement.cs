using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class GroupStatement : Statement
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

		public GroupStatement(Statements.GroupStatement statement)
		{
			ID = statement.ID;
			Area = statement.Area?.ID;
			Concept = statement.Concept?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(ID, conceptIdResolver.GetConceptById(Area), conceptIdResolver.GetConceptById(Concept));
		}
	}
}
