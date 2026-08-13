using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Classification.Xml
{
	/// <summary>XML surrogate of the <see cref="Statements.IsStatement"/> statement.</summary>
	[XmlType("Is")]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		/// <summary>Identifier of the ancestor concept.</summary>
		[XmlAttribute]
		public String Ancestor
		{ get; set; }

		/// <summary>Identifier of the descendant concept.</summary>
		[XmlAttribute]
		public String Descendant
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public IsStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public IsStatement(Statements.IsStatement statement)
			: base(statement)
		{
			Ancestor = statement.Ancestor?.ID;
			Descendant = statement.Descendant?.ID;
		}

		#endregion

		/// <summary>Rebuilds the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		protected override Statements.IsStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(ID, conceptIdResolver.GetConceptById(Ancestor), conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
