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

		public GroupStatement(Statements.GroupStatement statement, LoadIdResolver conceptIdResolver)
		{
			Area = conceptIdResolver.GetConceptId(statement.Area);
			Concept = conceptIdResolver.GetConceptId(statement.Concept);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(conceptIdResolver.GetConceptById(Area), conceptIdResolver.GetConceptById(Concept));
		}
	}
}
