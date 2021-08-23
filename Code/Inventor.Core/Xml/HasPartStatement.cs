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

		public HasPartStatement(Statements.HasPartStatement statement)
		{
			ID = statement.ID;
			Whole = statement.Whole?.ID;
			Part = statement.Part?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(ID, conceptIdResolver.GetConceptById(Whole), conceptIdResolver.GetConceptById(Part));
		}
	}
}
