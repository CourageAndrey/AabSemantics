using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Answers;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Questions;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Questions
{
	/// <summary>Asks how two values compare.</summary>
	public class ComparisonQuestion : Question
	{
		#region Properties

		/// <summary>The left-hand value.</summary>
		public IConcept LeftValue
		{ get; set; }

		/// <summary>The right-hand value.</summary>
		public IConcept RightValue
		{ get; set; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">Either value is <c>null</c>.</exception>
		public ComparisonQuestion(IConcept leftValue, IConcept rightValue, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			LeftValue = leftValue.EnsureNotNull(nameof(leftValue));
			RightValue = rightValue.EnsureNotNull(nameof(rightValue));
		}

		/// <summary>
		/// Finds the comparison between the two values, normalising each matched statement to the
		/// asked operand order and recursing through the values' ancestors when nothing matches directly.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>An answer naming the comparison sign, or the "unknown" answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<ComparisonQuestion, ComparisonStatement>()
				.WithTransitives(s => Task.FromResult(s.Count == 0), GetNestedQuestions)
				.Where(s => (s.LeftValue == LeftValue && s.RightValue == RightValue) || (s.RightValue == LeftValue && s.LeftValue == RightValue))
				.SelectCustomAsync(CreateAnswer)
				.ConfigureAwait(false);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			return statements.Count > 0
				? createAnswer(statements.First(), context)
				: ProcessChildAnswers(context, childAnswers);
		}

		private static StatementAnswer createAnswer(ComparisonStatement statement, IQuestionProcessingContext<ComparisonQuestion> context, ICollection<IStatement> transitiveStatements = null)
		{
			var resultStatement = statement.SwapOperandsToMatchOrder(context.Question);

			var text = new UnstructuredContainer();
			text.Append(resultStatement.DescribeTrue());

			var explanation = transitiveStatements == null
				? new Explanation(statement)
				: new Explanation(transitiveStatements);

			return new StatementAnswer(resultStatement, text, explanation);
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<ComparisonQuestion> context)
		{
			foreach (var statement in context.SemanticNetwork.Statements.Enumerate<ComparisonStatement>(context.ActiveContexts))
			{
				IConcept newLeftValue = null;
				if (statement.LeftValue == LeftValue)
				{
					newLeftValue = statement.RightValue;
				}
				else if (statement.RightValue == LeftValue)
				{
					newLeftValue = statement.LeftValue;
				}

				if (newLeftValue != null)
				{
					var involvedValues = new HashSet<IConcept>(context.ActiveContexts
						.OfType<IQuestionProcessingContext<ComparisonQuestion>>()
						.Select(c => c.Question.LeftValue));

					if (!involvedValues.Contains(newLeftValue))
					{
						yield return new NestedQuestion(new ComparisonQuestion(newLeftValue, RightValue), new IStatement[] { statement });
					}
				}
			}
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ChildAnswer> childAnswers)
		{
			foreach (var answer in childAnswers)
			{
				var childStatement = (answer.Answer as StatementAnswer)?.Result as ComparisonStatement;
				if (childStatement != null)
				{
					var transitiveStatement = (ComparisonStatement) answer.TransitiveStatements.Single();
					var intermediateValue = new[] { childStatement.LeftValue, childStatement.RightValue }.Intersect(new[] { transitiveStatement.LeftValue, transitiveStatement.RightValue }).Single();
					if ((childStatement.LeftValue == intermediateValue) == (transitiveStatement.LeftValue == intermediateValue))
					{
						transitiveStatement = transitiveStatement.SwapOperands();
					}

					var resultSign = ComparisonSigns.CompareThreeValues(childStatement.ComparisonSign, transitiveStatement.ComparisonSign);
					if (resultSign != null)
					{
						var transitiveStatements = new List<IStatement>(answer.TransitiveStatements);
						transitiveStatements.AddRange(answer.Answer.Explanation.Statements);

						return createAnswer(
							new ComparisonStatement(null, LeftValue, RightValue, resultSign),
							context,
							transitiveStatements);
					}
				}
			}

			return Answer.CreateUnknown();
		}
	}
}
