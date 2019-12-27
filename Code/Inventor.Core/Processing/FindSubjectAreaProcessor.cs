using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class FindSubjectAreaProcessor : QuestionProcessor<FindSubjectAreaQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, FindSubjectAreaQuestion question)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var propositions = knowledgeBase.Propositions.OfType<SubjectArea>().Where(c => c.Concept == question.Concept);
            if (propositions.Any())
            {
                var result = new FormattedText();
                foreach (var proposition in propositions)
                {
                    result.Add(() => language.SubjectArea, new Dictionary<string, INamed>
                    {
                        { "#CONCEPT#", question.Concept },
                        { "#AREA#", proposition.Area },
                    });
                }
                return result;
            }
            else
            {
                return AnswerHelper.CreateUnknown();
            }
        }
    }
}
