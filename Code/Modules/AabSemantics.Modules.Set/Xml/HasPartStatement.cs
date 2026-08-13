using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Statements.HasPartStatement"/>, storing its concepts by identifier.</summary>
	[XmlType("HasPart")]
	public class HasPartStatement : Statement<Statements.HasPartStatement>
	{
		#region Properties

		/// <summary>Identifier of the whole concept.</summary>
		[XmlAttribute]
		public String Whole
		{ get; set; }

		/// <summary>Identifier of the part concept.</summary>
		[XmlAttribute]
		public String Part
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public HasPartStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public HasPartStatement(Statements.HasPartStatement statement)
			: base(statement)
		{
			Whole = statement.Whole?.ID;
			Part = statement.Part?.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.HasPartStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.HasPartStatement(ID, conceptIdResolver.GetConceptById(Whole), conceptIdResolver.GetConceptById(Part));
		}
	}
}
