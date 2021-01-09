using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class HasSignsProcessor : QuestionProcessor<HasSignsQuestion>
	{
		public override Answer Process(ProcessingContext<HasSignsQuestion> context)
		{
			var question = context.QuestionX;
			var statements = HasSignStatement.GetSigns(context.KnowledgeBase.Statements, question.Concept, question.Recursive);
			return new Answer(
				statements.Any(),
				new FormattedText(
					() => string.Format(statements.Any() ? context.Language.Answers.HasSignsTrue : context.Language.Answers.HasSignsFalse, question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<string, INamed>
					{
						{ "#CONCEPT#", question.Concept },
					}),
				new Explanation(statements));
		}
	}
}
