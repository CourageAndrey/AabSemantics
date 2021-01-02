using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class EnumerateSignsProcessor : QuestionProcessor<EnumerateSignsQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, EnumerateSignsQuestion question)
		{
			var signs = HasSignStatement.GetSigns(knowledgeBase.Statements, question.Concept, question.Recursive).Select(hs => hs.Sign).ToList();
			if (signs.Count > 0)
			{
				var language = LanguageEx.CurrentEx.Answers;
				string format;
				var parameters = signs.Enumerate(out format);
				parameters["#CONCEPT#"] = question.Concept;
				return new FormattedText(
					() => string.Format(language.ConceptSigns, question.Recursive ? language.RecursiveTrue : language.RecursiveFalse, format),
					parameters);
			}
			else
			{
				return AnswerHelper.CreateUnknown();
			}
		}
	}
}
