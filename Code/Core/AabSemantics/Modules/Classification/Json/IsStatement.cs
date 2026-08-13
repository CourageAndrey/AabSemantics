using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Classification.Json
{
	/// <summary>JSON surrogate of the <see cref="Statements.IsStatement"/> statement.</summary>
	[DataContract]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		/// <summary>Identifier of the ancestor concept.</summary>
		[DataMember]
		public String Ancestor
		{ get; set; }

		/// <summary>Identifier of the descendant concept.</summary>
		[DataMember]
		public String Descendant
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public IsStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public IsStatement(Statements.IsStatement statement)
			: base(statement)
		{
			Ancestor = statement.Ancestor.ID;
			Descendant = statement.Descendant.ID;
		}

		#endregion

		/// <summary>Rebuilds the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		protected override Statements.IsStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(
				ID,
				conceptIdResolver.GetConceptById(Ancestor),
				conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
