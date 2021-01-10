using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class IsValueProcessor : QuestionProcessor<IsValueQuestion>
	{
		public override IAnswer Process(IProcessingContext<IsValueQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<SignValueStatement>().Where(r => r.Value == question.Concept).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.ValueTrue) : () => context.Language.Answers.ValueFalse,
					new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
