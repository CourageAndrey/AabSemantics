using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class CheckStatementProcessor : QuestionProcessor<CheckStatementQuestion>
	{
		protected override FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, CheckStatementQuestion question)
		{
			var assertion = knowledgeBase.Statements.FirstOrDefault(p => p.Equals(question.Statement));
			var result = new FormattedText(
				() => "#ANSWER#.",
				new Dictionary<string, INamed> { { "#ANSWER#", assertion != null ? knowledgeBase.True : knowledgeBase.False } });
			result.Add(assertion != null ? assertion.DescribeTrue() : question.Statement.DescribeFalse());
			return result;
		}
	}
}
