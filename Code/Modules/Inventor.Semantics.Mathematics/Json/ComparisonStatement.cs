using System;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Mathematics.Json
{
	[Serializable]
	public class ComparisonStatement : Serialization.Json.Statement<Statements.ComparisonStatement>
	{
		#region Properties

		public String LeftValue
		{ get; private set; }

		public String RightValue
		{ get; private set; }

		public String ComparisonSign
		{ get; private set; }

		#endregion

		#region Constructors

		public ComparisonStatement()
		{ }

		public ComparisonStatement(Statements.ComparisonStatement statement)
		{
			ID = statement.ID;
			LeftValue = statement.LeftValue.ID;
			RightValue = statement.RightValue.ID;
			ComparisonSign = statement.ComparisonSign.ID;
		}

		#endregion

		protected override Statements.ComparisonStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ComparisonStatement(
				ID,
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				conceptIdResolver.GetConceptById(ComparisonSign));
		}
	}
}
