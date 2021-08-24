using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
					language => language.Answers.HasSignTrue + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					language => language.Answers.HasSignFalse + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					});
		}
	}
}
