using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class FindSubjectAreaProcessor : QuestionProcessor<FindSubjectAreaQuestion>
	{
		public override Answer Process(ProcessingContext<FindSubjectAreaQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<GroupStatement>().Where(c => c.Concept == question.Concept).ToList();
			if (statements.Any())
			{
				var result = new FormattedText();
				foreach (var statement in statements)
				{
					result.Add(() => context.Language.Answers.SubjectArea, new Dictionary<String, INamed>
					{
						{ "#CONCEPT#", question.Concept },
						{ "#AREA#", statement.Area },
					});
				}
				return new Answer(
					statements.Select(s => s.Area),
					result,
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
