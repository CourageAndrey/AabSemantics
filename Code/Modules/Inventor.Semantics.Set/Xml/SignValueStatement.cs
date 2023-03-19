using System;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType("SignValue")]
	public class SignValueStatement : Statement
	{
		#region Properties

		[XmlAttribute]
		public String Concept
		{ get; set; }

		[XmlAttribute]
		public String Sign
		{ get; set; }

		[XmlAttribute]
		public String Value
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueStatement()
		{ }

		public SignValueStatement(Set.Statements.SignValueStatement statement)
		{
			ID = statement.ID;
			Concept = statement.Concept?.ID;
			Sign = statement.Sign?.ID;
			Value = statement.Value?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.SignValueStatement(ID, conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign), conceptIdResolver.GetConceptById(Value));
		}
	}
}
