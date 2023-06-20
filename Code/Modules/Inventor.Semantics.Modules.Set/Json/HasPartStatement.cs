using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Set.Json
{
	[DataContract]
	public class HasPartStatement : Serialization.Json.Statement<Statements.HasPartStatement>
	{
		#region Properties

		[DataMember]
		public String Whole
		{ get; set; }

		[DataMember]
		public String Part
		{ get; set; }

		#endregion

		#region Constructors

		public HasPartStatement()
			: base()
		{ }

		public HasPartStatement(Statements.HasPartStatement statement)
			: base(statement)
		{
			Whole = statement.Whole.ID;
			Part = statement.Part.ID;
		}

		#endregion

		protected override Statements.HasPartStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(
				ID,
				conceptIdResolver.GetConceptById(Whole),
				conceptIdResolver.GetConceptById(Part));
		}
	}
}
