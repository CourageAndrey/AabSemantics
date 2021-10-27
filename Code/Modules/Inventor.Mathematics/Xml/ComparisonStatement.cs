using System;
using System.Xml.Serialization;

using Inventor.Semantics;
using Inventor.Semantics.Xml;

namespace Inventor.Mathematics.Xml
{
	[XmlType("Comparison")]
	public class ComparisonStatement : Statement
	{
		#region Properties

		[XmlAttribute]
		public String LeftValue
		{ get; set; }

		[XmlAttribute]
		public String RightValue
		{ get; set; }

		[XmlAttribute]
		public String ComparisonSign
		{ get; set; }

		#endregion

		#region Constructors

		public ComparisonStatement()
		{ }

		public ComparisonStatement(Statements.ComparisonStatement statement)
		{
			ID = statement.ID;
			LeftValue = statement.LeftValue?.ID;
			RightValue = statement.RightValue?.ID;
			ComparisonSign = statement.ComparisonSign?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ComparisonStatement(ID, conceptIdResolver.GetConceptById(LeftValue), conceptIdResolver.GetConceptById(RightValue), conceptIdResolver.GetConceptById(ComparisonSign));
		}
	}
}
