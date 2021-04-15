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
	public sealed class FindSubjectAreaProcessor : QuestionProcessor<FindSubjectAreaQuestion, GroupStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<FindSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			if (statements.Any())
			{
				var result = new FormattedText();
				foreach (var statement in statements)
				{
					result.Add(() => context.Language.Answers.SubjectArea, new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
						{ Strings.ParamArea, statement.Area },
					});
				}
				return new ConceptsAnswer(
					statements.Select(s => s.Area).ToList(),
					result,
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<FindSubjectAreaQuestion> context, GroupStatement statement)
		{
			return statement.Concept == context.Question.Concept;
		}
	}
}
