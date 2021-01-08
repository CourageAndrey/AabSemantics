using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class DescribeSubjectAreaProcessor : QuestionProcessor<DescribeSubjectAreaQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, DescribeSubjectAreaQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<GroupStatement>().Where(c => c.Area == question.Concept);
			if (statements.Any())
			{
				string format;
				var parameters = statements.Select(r => r.Concept).ToList().Enumerate(out format);
				parameters.Add("#AREA#", question.Concept);
				return new FormattedText(() => language.Answers.SubjectAreaConcepts + format + ".", parameters);
			}
			else
			{
				return AnswerHelper.CreateUnknown(language);
			}
		}
	}
}
