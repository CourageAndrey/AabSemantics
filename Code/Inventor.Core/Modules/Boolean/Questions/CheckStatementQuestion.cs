using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Text.Containers;

namespace Inventor.Core.Questions
{
	public class CheckStatementQuestion : Question
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts);

			IEnumerable<IStatement> statements;
			var parentChild = Statement as IParentChild<IConcept>;
			if (parentChild != null)
			{
				statements = allStatements.FindPath(Statement.GetType(), parentChild.Parent, parentChild.Child);
			}
			else
			{
				var statement = allStatements.FirstOrDefault(p => p.Equals(Statement));
				statements = statement != null ? new[] { statement } : Array.Empty<IStatement>();
			}

			var result = new UnstructuredContainer();
			Boolean isTrue = statements.Any();
			result.Append(
				language => Strings.ParamAnswer,
				new Dictionary<String, IKnowledge> { { Strings.ParamAnswer, isTrue.ToLogicalValue() } });
			result.Append(isTrue ? Statement.DescribeTrue() : Statement.DescribeFalse());
			return new BooleanAnswer(
				isTrue,
				result,
				new Explanation(statements));
		}
	}
}
