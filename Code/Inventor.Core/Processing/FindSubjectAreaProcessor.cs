using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class FindSubjectAreaProcessor : QuestionProcessor<FindSubjectAreaQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, FindSubjectAreaQuestion question, ILanguageEx language)
		{
			var statements = knowledgeBase.Statements.OfType<GroupStatement>().Where(c => c.Concept == question.Concept);
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
				return result;
			}
			else
			{
				return AnswerHelper.CreateUnknown(language);
			}
		}
	}
}
