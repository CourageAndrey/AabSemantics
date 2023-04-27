using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Boolean.Questions
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
			Statement = statement.EnsureNotNull(nameof(statement));
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
			System.Boolean isTrue = statements.Any();
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
