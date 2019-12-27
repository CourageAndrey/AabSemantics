using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, EnumerateContainersQuestion question)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var propositions = knowledgeBase.Propositions.OfType<Composition>().Where(c => c.Child == question.Concept);
            if (propositions.Any())
            {
                string format;
                var parameters = propositions.Select(r => r.Parent).ToList().Enumerate(out format);
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
