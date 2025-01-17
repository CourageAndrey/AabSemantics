using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AabSemantics.Serialization.Xml
{
	[XmlType]
	public class CustomStatement : Statement<Statements.CustomStatement>
	{
		#region Properties

		[XmlElement]
		public String Type
		{ get; set; }

		[XmlElement]
		public List<KeyedConcept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
		{ }

		public CustomStatement(Statements.CustomStatement statement)
			: base(statement)
		{
			Type = statement.Type;
			Concepts = statement.Concepts.Select(c => new KeyedConcept(c.Key, c.Value.ID)).ToList();
		}

		#endregion

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

	[XmlType]
	public class KeyedConcept
	{
		#region Properties

		[XmlAttribute]
		public String Key
		{ get; set; }

		[XmlAttribute]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public KeyedConcept()
			: this(null, null)
		{ }

		public KeyedConcept(String key, String concept)
		{
			Key = key;
			Concept = concept;
		}

		#endregion
	}
}
