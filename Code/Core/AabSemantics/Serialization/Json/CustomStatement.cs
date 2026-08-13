using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AabSemantics.Serialization.Json
{
	/// <summary>JSON surrogate of a run-time declared statement: its kind plus one concept identifier per role.</summary>
	[DataContract]
	public class CustomStatement : Statement<Statements.CustomStatement>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		[DataMember]
		public String Type
		{ get; set; }

		/// <summary>Concept identifiers filling the kind's roles, keyed by role name.</summary>
		[DataMember]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public CustomStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public CustomStatement(Statements.CustomStatement statement)
			: base(statement)
		{
			Type = statement.Type;
			Concepts = statement.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">The statement kind is not registered, or a concept identifier is unknown.</exception>
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
