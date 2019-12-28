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
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, IsSubjectAreaQuestion question)
        {
            bool yes = knowledgeBase.Statements.OfType<GroupStatement>().Any(sa => sa.Area == question.Area && sa.Concept == question.Concept);
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(
                yes ? new Func<string>(() => language.IsSubjectAreaTrue) : () => language.IsSubjectAreaFalse,
                new Dictionary<string, INamed>
                {
                    { "#AREA#", question.Area },
                    { "#CONCEPT#", question.Concept },
                });
        }
    }
}
