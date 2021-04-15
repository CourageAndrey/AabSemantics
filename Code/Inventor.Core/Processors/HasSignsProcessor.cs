using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion, HasSignStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<HasSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignsTrue : context.Language.Answers.HasSignsFalse, context.Question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<HasSignsQuestion> context, HasSignStatement statement)
		{
			return statement.Concept == context.Question.Concept;
		}

		protected override Boolean AreEnoughToAnswer(IQuestionProcessingContext<HasSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			return !context.Question.Recursive;
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignsQuestion> context)
		{
			if (!context.Question.Recursive) yield break;

			var activeContexts = context.GetHierarchy();
			var alreadyViewedConcepts = new HashSet<IConcept>(activeContexts.OfType<IQuestionProcessingContext<HasSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(activeContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new HasSignsQuestion(parent, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
