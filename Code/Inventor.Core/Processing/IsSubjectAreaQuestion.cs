using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class IsSubjectAreaProcessor : QuestionProcessor<IsSubjectAreaQuestion>
	{
		public override Answer Process(ProcessingContext<IsSubjectAreaQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<GroupStatement>().Where(sa => sa.Area == question.Area && sa.Concept == question.Concept).ToList();
			return new Answer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.IsSubjectAreaTrue) : () => context.Language.Answers.IsSubjectAreaFalse,
					new Dictionary<String, INamed>
					{
						{ "#AREA#", question.Area },
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
