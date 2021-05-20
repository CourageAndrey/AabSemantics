using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class ComparisonQuestionProcessor : QuestionProcessor<ComparisonQuestion, ComparisonStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements)
		{
			if (statements.Any())
			{
				var statement = statements.First();
				var text = new FormattedText();
				text.Add(statement.DescribeTrue(context.Language));

				return new StatementAnswer(
					statement,
					text,
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override bool DoesStatementMatch(IQuestionProcessingContext<ComparisonQuestion> context, ComparisonStatement statement)
		{
			return	(statement.LeftValue == context.Question.LeftValue && statement.RightValue == context.Question.RightValue) ||
					(statement.RightValue == context.Question.LeftValue && statement.LeftValue == context.Question.RightValue);
		}
	}
}
