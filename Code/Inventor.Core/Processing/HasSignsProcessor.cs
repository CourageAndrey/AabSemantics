using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, HasSignsQuestion question)
        {
            bool yes = HasSign.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive).Select(hs => hs.Sign).Any();
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(
                () => string.Format(yes ? language.HasSignsTrue : language.HasSignsFalse, question.Recursive ? language.RecursiveTrue : language.RecursiveFalse),
                new Dictionary<string, INamed>
                {
                    { "#CONCEPT#", question.Concept },
                });
        }
    }
}
