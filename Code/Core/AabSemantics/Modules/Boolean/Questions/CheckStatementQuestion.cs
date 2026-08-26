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
	/// <summary>
	/// Asks whether a given statement holds. Works for any statement type, which is why it is
	/// meant to replace the per-statement <c>Is*</c> questions.
	/// </summary>
	public class CheckStatementQuestion : Question
	{
		#region Properties

		/// <summary>The statement whose truth is being checked.</summary>
		public IStatement Statement
		{ get; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="statement">Statement to check.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="statement"/> is <c>null</c>.</exception>
		public CheckStatementQuestion(IStatement statement, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Statement = statement.EnsureNotNull(nameof(statement));
		}

		/// <summary>
		/// Answers yes or no. Hierarchical statements are proved transitively, by searching for a
		/// chain of relations from parent to child; other statements only match an exactly equal one.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>A yes/no answer whose explanation holds the proving statements.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts);

			IEnumerable<IStatement> statements;
			var parentChild = Statement as IParentChild<IConcept>;
			if (parentChild != null)
			{
				statements = await allStatements.FindPathAsync(Statement.GetType(), parentChild.Parent, parentChild.Child, context.CancellationToken);
			}
			else
			{
				var statement = await allStatements.FirstOrDefaultAsync(p => p.Equals(Statement), context.CancellationToken);
				statements = statement != null ? new[] { statement } : Array.Empty<IStatement>();
			}

			var result = new UnstructuredContainer();
			System.Boolean isTrue = await statements.AnyAsync(cancellationToken: context.CancellationToken);
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
