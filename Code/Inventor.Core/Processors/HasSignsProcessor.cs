using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<HasSignsQuestion> context)
		{
			var question = context.QuestionX;
			var activeContexts = context.GetHierarchy();

			var statements = HasSignStatement.GetSigns(context.KnowledgeBase.Statements.Enumerate(activeContexts), question.Concept, question.Recursive);
			return new Answer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignsTrue : context.Language.Answers.HasSignsFalse, question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
