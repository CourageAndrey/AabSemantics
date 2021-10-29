﻿using System;
using System.Xml.Serialization;

using Inventor.Semantics.Xml;

namespace Inventor.Semantics.Modules.Classification.Xml
{
	[XmlType("Is")]
	public class IsStatement : Statement
	{
		#region Properties

		[XmlAttribute]
		public String Ancestor
		{ get; set; }

		[XmlAttribute]
		public String Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement()
		{ }

		public IsStatement(Statements.IsStatement statement)
		{
			ID = statement.ID;
			Ancestor = statement.Ancestor?.ID;
			Descendant = statement.Descendant?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(ID, conceptIdResolver.GetConceptById(Ancestor), conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
