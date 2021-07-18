using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
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

		public ComparisonStatement(Statements.ComparisonStatement statement, LoadIdResolver conceptIdResolver)
		{
			LeftValue = conceptIdResolver.GetConceptId(statement.LeftValue);
			RightValue = conceptIdResolver.GetConceptId(statement.RightValue);
			ComparisonSign = conceptIdResolver.GetConceptId(statement.ComparisonSign);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.ComparisonStatement(conceptIdResolver.GetConceptById(LeftValue), conceptIdResolver.GetConceptById(RightValue), conceptIdResolver.GetConceptById(ComparisonSign));
		}
	}
}
