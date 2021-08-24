using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class HasSignsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public HasSignsQuestion(IConcept concept, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
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
					language => language.Answers.HasSignsTrue + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					language => language.Answers.HasSignsFalse + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
					});
		}
	}
}
