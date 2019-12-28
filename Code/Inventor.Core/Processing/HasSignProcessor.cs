using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class HasSignProcessor : QuestionProcessor<HasSignQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, HasSignQuestion question)
        {
            bool yes = HasSign.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive).Select(hs => hs.Sign).Contains(question.Sign);
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(
                () => string.Format(yes ? language.HasSignTrue : language.HasSignFalse, question.Recursive ? language.RecursiveTrue : language.RecursiveFalse),
                new Dictionary<string, INamed>
                {
                    { "#CONCEPT#", question.Concept },
                    { "#SIGN#", question.Sign },
                });
        }
    }
}
