using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Propositions;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, IsPartOfQuestion question)
        {
            bool yes = knowledgeBase.Propositions.OfType<Composition>().Any(c => c.Parent == question.Parent && c.Child == question.Child);
            var language = LanguageEx.CurrentEx.Answers;
            return new FormattedText(yes ? new Func<string>(() => language.IsPartOfTrue) : () => language.IsPartOfFalse, new Dictionary<string, INamed>
            {
                {"#PARENT#", question.Parent},
                {"#CHILD#", question.Child},
            });
        }
    }
}
