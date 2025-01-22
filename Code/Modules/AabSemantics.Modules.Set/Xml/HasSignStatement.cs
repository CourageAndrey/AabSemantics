using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
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

		public HasSignStatement(Statements.HasSignStatement statement)
			: base(statement)
		{
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
