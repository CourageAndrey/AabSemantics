using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsPartOfQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<ConsistsOfStatement>(activeContexts).Where(c => c.Whole == question.Parent && c.Part == question.Child).ToList();
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
