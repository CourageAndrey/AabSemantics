using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class CheckPropositionProcessor : QuestionProcessor<CheckPropositionQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, CheckPropositionQuestion question)
        {
            var assertion = knowledgeBase.Propositions.FirstOrDefault(p => p.Equals(question.Proposition));
            var result = new FormattedText(
                () => "#ANSWER#.",
                new Dictionary<string, INamed> { { "#ANSWER#", assertion != null ? knowledgeBase.True : knowledgeBase.False } });
            result.Add(assertion != null ? assertion.DescribeTrue() : question.Proposition.DescribeFalse());
            return result;
        }
    }
}
