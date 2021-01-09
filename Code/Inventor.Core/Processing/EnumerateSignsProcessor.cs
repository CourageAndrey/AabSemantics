using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class EnumerateSignsProcessor : QuestionProcessor<EnumerateSignsQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, EnumerateSignsQuestion question, ILanguage language)
		{
			var statements = HasSignStatement.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive);
			if (statements.Any())
			{
				var signs = statements.Select(hs => hs.Sign);
				string format;
				var parameters = signs.Enumerate(out format);
				parameters["#CONCEPT#"] = question.Concept;
				return new FormattedText(
					() => string.Format(language.Answers.ConceptSigns, question.Recursive ? language.Answers.RecursiveTrue : language.Answers.RecursiveFalse, format),
					parameters);
			}
			else
			{
				return AnswerHelper.CreateUnknown(language);
			}
		}
	}
}
