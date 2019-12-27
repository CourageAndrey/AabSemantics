using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class IsSignProcessor : QuestionProcessor<IsSignQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, IsSignQuestion question)
        {
            bool yes = knowledgeBase.Propositions.OfType<HasSign>().FirstOrDefault(r => r.Sign == question.Concept) != null;
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(
                yes ? new Func<string>(() => language.SignTrue) : () => language.SignFalse,
                new Dictionary<string, INamed>
                {
                    { "#CONCEPT#", question.Concept },
                });
        }
    }
}
