using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Xml
{
	[XmlType]
	public class CustomStatement : Statement<Statements.CustomStatement>
	{
		#region Properties

		[XmlElement]
		public String Type
		{ get; set; }

		[XmlElement]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
		{ }

		public CustomStatement(Statements.CustomStatement statement)
			: base(statement)
		{
			Type = statement.Type;
			Concepts = statement.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
		}

		#endregion

		protected override Statements.CustomStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.CustomStatement(
				ID,
				Type,
				//String formatTrue
				,
				//String formatFalse
				,
				//String formatQuestion
				,
				//LocalizedString name
				,
				//LocalizedString hint = null
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Value)));
		}
	}
}
