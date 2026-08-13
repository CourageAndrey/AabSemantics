using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>XML surrogate of a run-time declared statement: its kind plus one concept identifier per role.</summary>
	[XmlType]
	public class CustomStatement : Statement<Statements.CustomStatement>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		[XmlElement]
		public String Type
		{ get; set; }

		/// <summary>Concept identifiers filling the kind's roles, keyed by role name.</summary>
		[XmlElement]
		public List<KeyedConcept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public CustomStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public CustomStatement(Statements.CustomStatement statement)
			: base(statement)
		{
			Type = statement.Type;
			Concepts = statement.Concepts.Select(c => new KeyedConcept(c.Key, c.Value.ID)).ToList();
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
					c => conceptIdResolver.GetConceptById(c.Concept)));
		}
	}

	/// <summary>One role-name and concept-identifier pair; XML has no dictionary form, so roles are written as a list.</summary>
	[XmlType]
	public class KeyedConcept
	{
		#region Properties

		/// <summary>Role name.</summary>
		[XmlAttribute]
		public String Key
		{ get; set; }

		/// <summary>Identifier of the concept filling the role.</summary>
		[XmlAttribute]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty pair, as required by the XML serializer.</summary>
		public KeyedConcept()
			: this(null, null)
		{ }

		/// <summary>Creates a role-and-concept pair.</summary>
		/// <param name="key">Role name.</param>
		/// <param name="concept">Identifier of the concept filling the role.</param>
		public KeyedConcept(String key, String concept)
		{
			Key = key;
			Concept = concept;
		}

		#endregion
	}
}
