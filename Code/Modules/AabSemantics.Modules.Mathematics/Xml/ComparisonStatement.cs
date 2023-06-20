using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Mathematics.Xml
{
	[XmlType("Comparison")]
	public class ComparisonStatement : Statement<Statements.ComparisonStatement>
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

		protected override Statements.ComparisonStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ComparisonStatement(ID, conceptIdResolver.GetConceptById(LeftValue), conceptIdResolver.GetConceptById(RightValue), conceptIdResolver.GetConceptById(ComparisonSign));
		}
	}
}
