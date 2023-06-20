using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Classification.Xml
{
	[XmlType("Is")]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		[XmlAttribute]
		public String Ancestor
		{ get; set; }

		[XmlAttribute]
		public String Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement()
		{ }

		public IsStatement(Statements.IsStatement statement)
		{
			ID = statement.ID;
			Ancestor = statement.Ancestor?.ID;
			Descendant = statement.Descendant?.ID;
		}

		#endregion

		protected override Statements.IsStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(ID, conceptIdResolver.GetConceptById(Ancestor), conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
