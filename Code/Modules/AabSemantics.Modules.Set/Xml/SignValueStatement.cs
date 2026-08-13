using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Statements.SignValueStatement"/>, storing its concepts by identifier.</summary>
	[XmlType("SignValue")]
	public class SignValueStatement : Statement<Statements.SignValueStatement>
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

		/// <summary>Identifier of the value concept.</summary>
		[XmlAttribute]
		public String Value
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public SignValueStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public SignValueStatement(Statements.SignValueStatement statement)
			: base(statement)
		{
			Concept = statement.Concept?.ID;
			Sign = statement.Sign?.ID;
			Value = statement.Value?.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.SignValueStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.SignValueStatement(ID, conceptIdResolver.GetConceptById(Concept), conceptIdResolver.GetConceptById(Sign), conceptIdResolver.GetConceptById(Value));
		}
	}
}
