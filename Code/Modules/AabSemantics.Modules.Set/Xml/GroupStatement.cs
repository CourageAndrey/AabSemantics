﻿using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType("Group")]
	public class GroupStatement : Statement<Statements.GroupStatement>
	{
		#region Properties

		[XmlAttribute]
		public String Area
		{ get; set; }

		[XmlAttribute]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public GroupStatement()
		{ }

		public GroupStatement(Statements.GroupStatement statement)
			: base(statement)
		{
			Area = statement.Area?.ID;
			Concept = statement.Concept?.ID;
		}

		#endregion

		protected override Statements.GroupStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.GroupStatement(ID, conceptIdResolver.GetConceptById(Area), conceptIdResolver.GetConceptById(Concept));
		}
	}
}
