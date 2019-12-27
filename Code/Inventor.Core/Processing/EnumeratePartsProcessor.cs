using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Propositions;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
    public sealed class EnumeratePartsProcessor : QuestionProcessor<EnumeratePartsQuestion>
    {
        protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, EnumeratePartsQuestion question)
        {
            var language = LanguageEx.CurrentEx.Answers;
            var propositions = knowledgeBase.Propositions.OfType<Composition>().Where(c => c.Parent == question.Concept);
            if (propositions.Any())
            {
                string format;
                var parameters = propositions.Select(r => r.Child).ToList().Enumerate(out format);
                parameters.Add("#PARENT#", question.Concept);
                return new FormattedText(() => language.EnumerateParts + format + ".", parameters);
            }
            else
            {
                return AnswerHelper.CreateUnknown();
            }
        }
    }
}
