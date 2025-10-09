using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Answers;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Questions;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Boolean.Questions
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

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts);

			IEnumerable<IStatement> statements;
			var parentChild = Statement as IParentChild<IConcept>;
			if (parentChild != null)
			{
				statements = await allStatements.FindPathAsync(Statement.GetType(), parentChild.Parent, parentChild.Child);
			}
			else
			{
				var statement = await allStatements.FirstOrDefaultAsync(p => p.Equals(Statement));
				statements = statement != null ? new[] { statement } : Array.Empty<IStatement>();
			}

			var result = new UnstructuredContainer();
			System.Boolean isTrue = await statements.AnyAsync();
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
