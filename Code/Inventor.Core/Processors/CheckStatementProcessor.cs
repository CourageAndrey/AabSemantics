using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class CheckStatementProcessor : QuestionProcessor<CheckStatementQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<CheckStatementQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();
			var allStatements = context.KnowledgeBase.Statements.Enumerate(activeContexts);

			IEnumerable<IStatement> statements;
			var parentChild = question.Statement as IParentChild<IConcept>;
			if (parentChild != null)
			{
				statements = allStatements.FindPath(question.Statement.GetType(), parentChild.Parent, parentChild.Child);
			}
			else
			{
				var statement = allStatements.FirstOrDefault(p => p.Equals(question.Statement));
				statements = statement != null ? new[] { statement } : new IStatement[0];
			}

			var result = new FormattedText(
				() => Strings.ParamAnswer,
				new Dictionary<String, INamed> { { Strings.ParamAnswer, statements.Any() ? context.KnowledgeBase.True : context.KnowledgeBase.False } });
			result.Add(statements.Any() ? question.Statement.DescribeTrue(context.Language) : question.Statement.DescribeFalse(context.Language));
			return new BooleanAnswer(
				statements.Any(),
				result,
				new Explanation(statements));
		}
	}
}
