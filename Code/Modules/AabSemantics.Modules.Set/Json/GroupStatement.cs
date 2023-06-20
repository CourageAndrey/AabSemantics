using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	[DataContract]
	public class GroupStatement : Serialization.Json.Statement<Statements.GroupStatement>
	{
		#region Properties

		[DataMember]
		public String Area
		{ get; set; }

		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
			: base()
		{ }

		public GroupStatement(Statements.GroupStatement statement)
			: base(statement)
		{
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
