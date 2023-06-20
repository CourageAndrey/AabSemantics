using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Set.Json
{
	[DataContract]
	public class HasSignStatement : Serialization.Json.Statement<Statements.HasSignStatement>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		[DataMember]
		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		public HasSignStatement()
			: base()
		{ }

		public HasSignStatement(Statements.HasSignStatement statement)
			: base(statement)
		{
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
