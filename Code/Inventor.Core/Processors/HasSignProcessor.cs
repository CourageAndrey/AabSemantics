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
	public sealed class HasSignProcessor : QuestionProcessor<HasSignQuestion, HasSignStatement>
	{
		public override IAnswer Process(IQuestionProcessingContext<HasSignQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();
			var allStatements = context.KnowledgeBase.Statements.Enumerate(activeContexts);

			var statements = HasSignStatement.GetSigns(allStatements, question.Concept, question.Recursive).Where(statement => DoesStatementMatch(context, statement)).ToList();
			return CreateAnswer(context, statements);
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<HasSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignTrue : context.Language.Answers.HasSignFalse, context.Question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
						{ Strings.ParamSign, context.Question.Sign },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<HasSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == context.Question.Sign;
		}
	}
}
