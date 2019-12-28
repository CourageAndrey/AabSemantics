using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class IsValueProcessor : QuestionProcessor<IsValueQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, IsValueQuestion question)
        {
            bool yes = knowledgeBase.Statements.OfType<SignValue>().FirstOrDefault(r => r.Value == question.Concept) != null;
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(
                yes ? new Func<string>(() => language.ValueTrue) : () => language.ValueFalse,
                new Dictionary<string, INamed>
                {
                    { "#CONCEPT#", question.Concept },
                });
        }
    }
}
