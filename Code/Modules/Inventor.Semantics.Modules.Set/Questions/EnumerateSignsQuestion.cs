using System;
using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Set.Questions
{
	public class EnumerateSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateSignsQuestion, HasSignStatement>()
				.WithTransitives(
					statements => Recursive,
					question => question.Concept,
					newSubject => new EnumerateSignsQuestion(newSubject, true),
					needToAggregateTransitivesToStatements: true)
				.Where(s => s.Concept == Concept)
				.SelectAllConcepts(
					statement => statement.Sign,
					question => question.Concept,
					Semantics.Localization.Strings.ParamConcept,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.ConceptSigns + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ": ");
		}
	}
}
