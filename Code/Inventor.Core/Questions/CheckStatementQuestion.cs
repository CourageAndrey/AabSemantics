using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : Question<CheckStatementQuestion>
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public CheckStatementQuestion(IStatement statement, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}

		public override IAnswer Process(IQuestionProcessingContext<CheckStatementQuestion> context)
		{
			var question = context.Question;
			var allStatements = context.KnowledgeBase.Statements.Enumerate(context.ActiveContexts);

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
				new Dictionary<String, INamed> { { Strings.ParamAnswer, statements.Any() ? LogicalValues.True : LogicalValues.False } });
			result.Add(statements.Any() ? question.Statement.DescribeTrue(context.Language) : question.Statement.DescribeFalse(context.Language));
			return new BooleanAnswer(
				statements.Any(),
				result,
				new Explanation(statements));
		}
	}
}
