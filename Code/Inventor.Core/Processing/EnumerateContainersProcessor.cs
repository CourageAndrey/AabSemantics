﻿using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, EnumerateContainersQuestion question)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var statements = knowledgeBase.Statements.OfType<Composition>().Where(c => c.Child == question.Concept);
            if (statements.Any())
            {
                string format;
                var parameters = statements.Select(r => r.Parent).ToList().Enumerate(out format);
                parameters.Add("#CHILD#", question.Concept);
                return new FormattedText(() => language.EnumerateContainers + format + ".", parameters);
            }
            else
            {
                return AnswerHelper.CreateUnknown();
            }
        }
    }
}
