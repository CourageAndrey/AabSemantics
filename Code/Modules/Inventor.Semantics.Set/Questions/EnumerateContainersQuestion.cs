﻿using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Set.Questions
{
	public class EnumerateContainersQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateContainersQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateContainersQuestion, HasPartStatement>()
				.Where(s => s.Part == Concept)
				.SelectAllConcepts(
					statement => statement.Whole,
					question => question.Concept,
					Semantics.Localization.Strings.ParamChild,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.EnumerateContainers);
		}
	}
}
