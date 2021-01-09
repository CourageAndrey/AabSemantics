using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class FindSubjectAreaProcessor : QuestionProcessor<FindSubjectAreaQuestion>
	{
		protected override Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, FindSubjectAreaQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<GroupStatement>().Where(c => c.Concept == question.Concept).ToList();
			if (statements.Any())
			{
				var result = new FormattedText();
				foreach (var statement in statements)
				{
					result.Add(() => language.Answers.SubjectArea, new Dictionary<string, INamed>
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
				return Answer.CreateUnknown(language);
			}
		}
	}
}
