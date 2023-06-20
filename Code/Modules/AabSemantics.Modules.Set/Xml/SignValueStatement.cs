using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType("SignValue")]
	public class SignValueStatement : Statement<Statements.SignValueStatement>
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

		protected override Statements.SignValueStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.SignValueStatement(ID, conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign), conceptIdResolver.GetConceptById(Value));
		}
	}
}
