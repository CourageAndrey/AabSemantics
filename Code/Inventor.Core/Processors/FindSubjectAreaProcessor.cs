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
		public override IAnswer Process(IQuestionProcessingContext<FindSubjectAreaQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<GroupStatement>(activeContexts).Where(c => c.Concept == question.Concept).ToList();
			return CreateAnswer(context, statements);
		}

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
	}
}
