using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class EnumeratePartsProcessor : QuestionProcessor<EnumeratePartsQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, EnumeratePartsQuestion question, ILanguageEx language)
		{
			var statements = knowledgeBase.Statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == question.Concept);
			if (statements.Any())
			{
				string format;
				var parameters = statements.Select(r => r.Child).ToList().Enumerate(out format);
				parameters.Add("#PARENT#", question.Concept);
				return new FormattedText(() => language.Answers.EnumerateParts + format + ".", parameters);
			}
			else
			{
				return AnswerHelper.CreateUnknown(language);
			}
		}
	}
}
