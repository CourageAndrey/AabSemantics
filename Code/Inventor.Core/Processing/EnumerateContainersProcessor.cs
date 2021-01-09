using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class EnumerateContainersProcessor : QuestionProcessor<EnumerateContainersQuestion>
	{
		protected override Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, EnumerateContainersQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Child == question.Concept).ToList();
			if (statements.Any())
			{
				string format;
				var parameters = statements.Select(r => r.Parent).ToList().Enumerate(out format);
				parameters.Add("#CHILD#", question.Concept);
				return new Answer(
					statements.Select(s => s.Parent),
					new FormattedText(() => language.Answers.EnumerateContainers + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(language);
			}
		}
	}
}
