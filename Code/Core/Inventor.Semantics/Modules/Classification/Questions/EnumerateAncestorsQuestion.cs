﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Modules.Classification.Questions
{
	public class EnumerateAncestorsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateAncestorsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateAncestorsQuestion, IsStatement>()
				.Where(s => s.Descendant == Concept)
				.SelectAllConcepts(
					statement => statement.Ancestor,
					question => question.Concept,
					Strings.ParamChild,
					language => language.GetExtension<ILanguageClassificationModule>().Questions.Answers.EnumerateAncestors);
		}
	}
}
