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
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, IsSubjectAreaQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<GroupStatement>().Where(sa => sa.Area == question.Area && sa.Concept == question.Concept).ToList();
			return new FormattedText(
				statements.Any() ? new Func<string>(() => language.Answers.IsSubjectAreaTrue) : () => language.Answers.IsSubjectAreaFalse,
				new Dictionary<string, INamed>
				{
					{ "#AREA#", question.Area },
					{ "#CONCEPT#", question.Concept },
				});
		}
	}
}
