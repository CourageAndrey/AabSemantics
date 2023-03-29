﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Classification.Xml
{
	[XmlType]
	public class EnumerateAncestorsQuestion : Question<Questions.EnumerateAncestorsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateAncestorsQuestion()
		{ }

		public EnumerateAncestorsQuestion(Questions.EnumerateAncestorsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateAncestorsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateAncestorsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
