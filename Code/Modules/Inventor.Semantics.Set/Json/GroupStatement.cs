using System;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[Serializable]
	public class GroupStatement : Serialization.Json.Statement<Statements.GroupStatement>
	{
		#region Properties

		public String Area
		{ get; set; }

		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
		{ }

		public GroupStatement(Statements.GroupStatement statement)
		{
			ID = statement.ID;
			Area = statement.Area.ID;
			Concept = statement.Concept.ID;
		}

		#endregion

		protected override Statements.GroupStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(
				ID,
				conceptIdResolver.GetConceptById(Area),
				conceptIdResolver.GetConceptById(Concept));
		}
	}
}
