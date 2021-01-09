using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion>
	{
		public override Answer Process(ProcessingContext<IsPartOfQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == question.Parent && c.Child == question.Child).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(statements.Any() ? new Func<string>(() => context.Language.Answers.IsPartOfTrue) : () => context.Language.Answers.IsPartOfFalse, new Dictionary<string, INamed>
				{
					{ "#PARENT#", question.Parent },
					{ "#CHILD#", question.Child },
				}),
				new Explanation(statements));
		}
	}
}
