using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class HasSignStatement : Statement
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

		public HasSignStatement(Statements.HasSignStatement statement, LoadIdResolver conceptIdResolver)
		{
			Concept = conceptIdResolver.GetConceptId(statement.Concept);
			Sign = conceptIdResolver.GetConceptId(statement.Sign);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.HasSignStatement(conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign));
		}
	}
}
