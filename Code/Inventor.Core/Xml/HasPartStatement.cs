using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class HasPartStatement : Statement
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

		public HasPartStatement(Statements.HasPartStatement statement, LoadIdResolver conceptIdResolver)
		{
			Whole = conceptIdResolver.GetConceptId(statement.Whole);
			Part = conceptIdResolver.GetConceptId(statement.Part);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(conceptIdResolver.GetConceptById(Whole), conceptIdResolver.GetConceptById(Part));
		}
	}
}
