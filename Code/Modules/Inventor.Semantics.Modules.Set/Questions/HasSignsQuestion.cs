﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Set.Questions
{
	public class HasSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public System.Boolean Recursive
		{ get; }

		#endregion

		public HasSignsQuestion(IConcept concept, System.Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<HasSignsQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Recursive,
					question => question.Concept,
					newSubject => new HasSignsQuestion(newSubject, true))
				.Where(s => s.Concept == Concept)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.HasSignsTrue + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.HasSignsFalse + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
