using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class CheckStatementProcessor : QuestionProcessor<CheckStatementQuestion>
	{
		public override Answer Process(ProcessingContext<CheckStatementQuestion> context)
		{
			var question = context.QuestionX;
			var statement = context.KnowledgeBase.Statements.FirstOrDefault(p => p.Equals(question.Statement));
			var result = new FormattedText(
				() => "#ANSWER#.",
				new Dictionary<String, INamed> { { "#ANSWER#", statement != null ? context.KnowledgeBase.True : context.KnowledgeBase.False } });
			result.Add(statement != null ? statement.DescribeTrue(context.Language) : question.Statement.DescribeFalse(context.Language));
			return new Answer(
				statement != null,
				result,
				new Explanation(statement));
		}
	}
}
