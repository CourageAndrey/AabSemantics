using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
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

		public SignValueStatement(Statements.SignValueStatement statement, LoadIdResolver conceptIdResolver)
		{
			Concept = conceptIdResolver.GetConceptId(statement.Concept);
			Sign = conceptIdResolver.GetConceptId(statement.Sign);
			Value = conceptIdResolver.GetConceptId(statement.Value);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.SignValueStatement(conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign), conceptIdResolver.GetConceptById(Value));
		}
	}
}
