using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
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
			: base()
		{ }

		public SignValueStatement(Statements.SignValueStatement statement)
			: base(statement)
		{
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
