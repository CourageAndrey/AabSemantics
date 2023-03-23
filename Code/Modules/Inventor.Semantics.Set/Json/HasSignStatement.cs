using System;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class HasSignStatement : Serialization.Json.Statement<Statements.HasSignStatement>
	{
		#region Properties

		public String Concept
		{ get; set; }

		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement()
		{ }

		public HasSignStatement(Statements.HasSignStatement statement)
		{
			ID = statement.ID;
			Concept = statement.Concept.ID;
			Sign = statement.Sign.ID;
		}

		#endregion

		protected override Statements.HasSignStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasSignStatement(
				ID,
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign));
		}
	}
}
