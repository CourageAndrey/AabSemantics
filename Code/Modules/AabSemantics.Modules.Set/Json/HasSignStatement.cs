using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Statements.HasSignStatement"/>, storing its concepts by identifier.</summary>
	[DataContract]
	public class HasSignStatement : Serialization.Json.Statement<Statements.HasSignStatement>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String Concept
		{ get; set; }

		/// <summary>Identifier of the sign concept.</summary>
		[DataMember]
		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public HasSignStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public HasSignStatement(Statements.HasSignStatement statement)
			: base(statement)
		{
			Concept = statement.Concept.ID;
			Sign = statement.Sign.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.HasSignStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasSignStatement(
				ID,
				conceptIdResolver.GetConceptById(Concept),
				conceptIdResolver.GetConceptById(Sign));
		}
	}
}
