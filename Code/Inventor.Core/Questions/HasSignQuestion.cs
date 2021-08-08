using System;
using System.Collections.Generic;
using System.Linq;

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
				.From<HasSignQuestion, HasSignStatement>(s => s.Concept == Concept && s.Sign == Sign)
				.WithTransitives(s => s.Count == 0 && Recursive, GetNestedQuestions)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => language.Answers.HasSignTrue + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					language => language.Answers.HasSignFalse + (Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse) + ".",
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					})
				.Answer;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<HasSignQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.SemanticNetwork.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new HasSignQuestion(parent, question.Sign, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
