using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion>
	{
		public override IAnswer Process(IProcessingContext<IsPartOfQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == question.Parent && c.Child == question.Child).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(statements.Any() ? new Func<String>(() => context.Language.Answers.IsPartOfTrue) : () => context.Language.Answers.IsPartOfFalse, new Dictionary<String, INamed>
				{
					{ "#PARENT#", question.Parent },
					{ "#CHILD#", question.Child },
				}),
				new Explanation(statements));
		}
	}
}
