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
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsSubjectAreaProcessor : QuestionProcessor<IsSubjectAreaQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsSubjectAreaQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<GroupStatement>(activeContexts).Where(sa => sa.Area == question.Area && sa.Concept == question.Concept).ToList();
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					statements.Any() ? new Func<String>(() => context.Language.Answers.IsSubjectAreaTrue) : () => context.Language.Answers.IsSubjectAreaFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamArea, question.Area },
						{ Strings.ParamConcept, question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
