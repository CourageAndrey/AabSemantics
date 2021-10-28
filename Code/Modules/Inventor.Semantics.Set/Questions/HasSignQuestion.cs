using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Set.Questions
{
	public class HasSignQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public HasSignQuestion(IConcept concept, IConcept sign, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<HasSignQuestion, HasSignStatement>()
				.WithTransitives(
					statements => statements.Count == 0 && Recursive,
					question => question.Concept,
					newSubject => new HasSignQuestion(newSubject, Sign, true))
				.Where(s => s.Concept == Concept && s.Sign == Sign)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.HasSignTrue + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.HasSignFalse + (Recursive ? language.Questions.Answers.RecursiveTrue : language.Questions.Answers.RecursiveFalse) + ".",
					new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					});
		}
	}
}
