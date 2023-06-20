﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class ConceptAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public ConceptAnswer(Semantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = answer.Result.ID;
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.ConceptAnswer(
				conceptIdResolver.GetConceptById(Concept),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}