using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class IsSignProcessor : QuestionProcessor<IsSignQuestion>
	{
		public override IAnswer Process(IProcessingContext<IsSignQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<HasSignStatement>().Where(r => r.Sign == question.Concept).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.SignTrue) : () => context.Language.Answers.SignFalse,
					new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
