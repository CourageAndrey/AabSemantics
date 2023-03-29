﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Classification.Xml
{
	[XmlType]
	public class IsQuestion : Question<Questions.IsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Child
		{ get; set; }

		[XmlElement]
		public String Parent
		{ get; set; }

		#endregion

		#region Constructors

		public IsQuestion()
		{ }

		public IsQuestion(Questions.IsQuestion question)
			: base(question)
		{
			Child = question.Child.ID;
			Parent = question.Parent.ID;
		}

		#endregion

		protected override Questions.IsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
