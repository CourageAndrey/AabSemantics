using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AabSemantics.Serialization.Json
{
	[DataContract]
	public class CustomStatement : Statement<Statements.CustomStatement>
	{
		#region Properties

		[DataMember]
		public String Type
		{ get; set; }

		[DataMember]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
		{ }

		public CustomStatement(Statements.CustomStatement statement)
			: base(statement)
		{
			Type = statement.Type;
			Concepts = statement.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
		}

		#endregion

		protected override Statements.CustomStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.CustomStatement(
				ID,
				Type,
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Value)));
		}
	}
}
