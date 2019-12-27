using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class WhatProcessor : QuestionProcessor<WhatQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, WhatQuestion question)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var propositions = knowledgeBase.Propositions.OfType<Clasification>().Where(c => c.Child == question.Concept);
            if (propositions.Any())
            {
                var result = new FormattedText();
                foreach (var proposition in propositions)
                {
                    var difference = new List<SignValue>();
                    foreach (var sign in HasSign.GetSigns(knowledgeBase.Propositions, proposition.Parent, false))
                    {
                        var diff = SignValue.GetSignValue(knowledgeBase.Propositions, question.Concept, sign.Sign);
                        if (diff != null)
                        {
                            difference.Add(diff);
                        }
                    }
                    if (difference.Count > 0)
                    {
                        result.Add(() => language.IsDescriptionWithSign, new Dictionary<string, INamed>
                        {
                            { "#CHILD#", question.Concept },
                            { "#PARENT#", proposition.Parent },
                        });
                        foreach (var diff in difference)
                        {
                            result.Add(() => language.IsDescriptionWithSignValue, new Dictionary<string, INamed>
                            {
                                { "#SIGN#", diff.Sign },
                                { "#VALUE#", diff.Value },
                            });
                        }
                    }
                    else
                    {
                        result.Add(() => language.IsDescription, new Dictionary<string, INamed>
                        {
                            { "#CHILD#", question.Concept },
                            { "#PARENT#", proposition.Parent },
                        });
                    }
                    result.Add(() => string.Empty, new Dictionary<string, INamed>());
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
