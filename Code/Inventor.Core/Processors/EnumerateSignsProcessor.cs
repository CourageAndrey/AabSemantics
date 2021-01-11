using System;
using System.Linq;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumerateSignsProcessor : QuestionProcessor<EnumerateSignsQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			var question = context.QuestionX;
			var statements = HasSignStatement.GetSigns(context.KnowledgeBase.Statements, question.Concept, question.Recursive);
			if (statements.Any())
			{
				var signs = statements.Select(hs => hs.Sign);
				String format;
				var parameters = signs.Enumerate(out format);
				parameters["#CONCEPT#"] = question.Concept;
				return new Answer(
					signs,
					new FormattedText(
						() => string.Format(context.Language.Answers.ConceptSigns, question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse, format),
						parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
