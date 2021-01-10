using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class HasSignProcessor : QuestionProcessor<HasSignQuestion>
	{
		public override IAnswer Process(IProcessingContext<HasSignQuestion> context)
		{
			var question = context.QuestionX;
			var statements = HasSignStatement.GetSigns(context.KnowledgeBase.Statements, question.Concept, question.Recursive).Where(hasSign => hasSign.Sign == question.Sign).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignTrue : context.Language.Answers.HasSignFalse, question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", question.Concept },
						{ "#SIGN#", question.Sign },
					}),
				new Explanation(statements));
		}
	}
}
