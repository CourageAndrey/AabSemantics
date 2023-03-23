using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Set.Json
{
	[DataContract]
	public class SignValueStatement : Serialization.Json.Statement<Statements.SignValueStatement>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		[DataMember]
		public String Sign
		{ get; set; }

		[DataMember]
		public String Value
		{ get; set; }

		#endregion

		#region Constructors

		public SignValueStatement()
		{ }

		public SignValueStatement(Statements.SignValueStatement statement)
		{
			ID = statement.ID;
			Concept = statement.Concept.ID;
			Sign = statement.Sign.ID;
			Value = statement.Value.ID;
		}

		#endregion

		protected override Statements.SignValueStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.SignValueStatement(
				ID,
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign),
				conceptIdResolver.GetConceptById(Value));
		}
	}
}
