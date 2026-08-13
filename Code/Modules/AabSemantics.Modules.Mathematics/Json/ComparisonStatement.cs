using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Mathematics.Json
{
	/// <summary>JSON surrogate of a <see cref="Statements.ComparisonStatement"/>, storing all three concepts by identifier.</summary>
	[DataContract]
	public class ComparisonStatement : Serialization.Json.Statement<Statements.ComparisonStatement>
	{
		#region Properties

		/// <summary>Identifier of the left-hand value.</summary>
		[DataMember]
		public String LeftValue
		{ get; private set; }

		/// <summary>Identifier of the right-hand value.</summary>
		[DataMember]
		public String RightValue
		{ get; private set; }

		/// <summary>Identifier of the comparison sign concept.</summary>
		[DataMember]
		public String ComparisonSign
		{ get; private set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public ComparisonStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public ComparisonStatement(Statements.ComparisonStatement statement)
			: base(statement)
		{
			LeftValue = statement.LeftValue.ID;
			RightValue = statement.RightValue.ID;
			ComparisonSign = statement.ComparisonSign.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.ComparisonStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ComparisonStatement(
				ID,
				conceptIdResolver.GetConceptById(LeftValue),
				conceptIdResolver.GetConceptById(RightValue),
				conceptIdResolver.GetConceptById(ComparisonSign));
		}
	}
}
