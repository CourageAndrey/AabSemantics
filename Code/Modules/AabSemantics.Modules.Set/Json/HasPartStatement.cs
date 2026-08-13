using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Statements.HasPartStatement"/>, storing its concepts by identifier.</summary>
	[DataContract]
	public class HasPartStatement : Serialization.Json.Statement<Statements.HasPartStatement>
	{
		#region Properties

		/// <summary>Identifier of the whole concept.</summary>
		[DataMember]
		public String Whole
		{ get; set; }

		/// <summary>Identifier of the part concept.</summary>
		[DataMember]
		public String Part
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public HasPartStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public HasPartStatement(Statements.HasPartStatement statement)
			: base(statement)
		{
			Whole = statement.Whole.ID;
			Part = statement.Part.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.HasPartStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(
				ID,
				conceptIdResolver.GetConceptById(Whole),
				conceptIdResolver.GetConceptById(Part));
		}
	}
}
