using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
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

		public HasPartStatement(Statements.HasPartStatement statement)
			: base(statement)
		{
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
