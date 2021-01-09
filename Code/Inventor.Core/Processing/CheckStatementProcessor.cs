using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class CheckStatementProcessor : QuestionProcessor<CheckStatementQuestion>
	{
		protected override FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, CheckStatementQuestion question, ILanguage language)
		{
			var statement = knowledgeBase.Statements.FirstOrDefault(p => p.Equals(question.Statement));
			var result = new FormattedText(
				() => "#ANSWER#.",
				new Dictionary<string, INamed> { { "#ANSWER#", statement != null ? knowledgeBase.True : knowledgeBase.False } });
			result.Add(statement != null ? statement.DescribeTrue(language) : question.Statement.DescribeFalse(language));
			return result;
		}
	}
}
