using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class SignValueProcessor : QuestionProcessor<SignValueQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, SignValueQuestion question)
        {
            var signValues = knowledgeBase.Statements.OfType<SignValue>();
            var result = getSignValue(signValues, question.Concept, question.Sign, question.Concept);
            if (result != null)
            {
                return result;
            }
            else
            {
                var parents = Clasification.GetParentsTree(knowledgeBase.Statements, question.Concept);
                foreach (var parent in parents)
                {
                    result = getSignValue(signValues, parent, question.Sign, question.Concept);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return AnswerHelper.CreateUnknown();
        }

        private static FormattedText getSignValue(IEnumerable<SignValue> statements, Concept concept, Concept sign, Concept original)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var value = statements.FirstOrDefault(v => v.Concept == concept && v.Sign == sign);
            return value != null
                ? new FormattedText(
                    () => language.SignValue,
                    new Dictionary<string, INamed>
                    {
                        { "#CONCEPT#", original },
                        { "#SIGN#", sign },
                        { "#VALUE#", value.Value },
                        { "#DEFINED#", concept },
                    })
                : null;
        }
    }
}
