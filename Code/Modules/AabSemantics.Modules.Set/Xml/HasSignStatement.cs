using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Statements.HasSignStatement"/>, storing its concepts by identifier.</summary>
	[XmlType("HasSign")]
	public class HasSignStatement : Statement<Statements.HasSignStatement>
	{
		#region Properties

		/// <summary>Identifier of the concept.</summary>
		[XmlAttribute]
		public String Concept
		{ get; set; }

		/// <summary>Identifier of the sign concept.</summary>
		[XmlAttribute]
		public String Sign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public HasSignStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public HasSignStatement(Statements.HasSignStatement statement)
			: base(statement)
		{
			Concept = statement.Concept?.ID;
			Sign = statement.Sign?.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.HasSignStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasSignStatement(ID, conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign));
		}
	}
}
