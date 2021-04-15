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
		public override IAnswer Process(IQuestionProcessingContext<HasSignsQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = HasSignStatement.GetSigns(context.KnowledgeBase.Statements.Enumerate(activeContexts), question.Concept, question.Recursive);
			return CreateAnswer(context, statements);
		}

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
			throw new NotImplementedException("Need to change method structure!");
		}
	}
}
