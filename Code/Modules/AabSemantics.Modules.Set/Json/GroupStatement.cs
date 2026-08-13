using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	/// <summary>JSON surrogate of a <see cref="Statements.GroupStatement"/>, storing its concepts by identifier.</summary>
	[DataContract]
	public class GroupStatement : Serialization.Json.Statement<Statements.GroupStatement>
	{
		#region Properties

		/// <summary>Identifier of the subject area concept.</summary>
		[DataMember]
		public String Area
		{ get; set; }

		/// <summary>Identifier of the concept.</summary>
		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public GroupStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public GroupStatement(Statements.GroupStatement statement)
			: base(statement)
		{
			Area = statement.Area.ID;
			Concept = statement.Concept.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.GroupStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(
				ID,
				conceptIdResolver.GetConceptById(Area),
				conceptIdResolver.GetConceptById(Concept));
		}
	}
}
