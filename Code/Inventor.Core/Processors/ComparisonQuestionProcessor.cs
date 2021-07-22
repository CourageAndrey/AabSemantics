using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class ComparisonQuestionProcessor : QuestionProcessor<ComparisonQuestion, ComparisonStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements)
		{
			if (statements.Any())
			{
				return createAnswer(statements.First(), context);
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static StatementAnswer createAnswer(ComparisonStatement statement, IQuestionProcessingContext<ComparisonQuestion> context, ICollection<IStatement> transitiveStatements = null)
		{
			var resultStatement = statement.SwapOperandsToMatchOrder(context.Question);

			var text = new FormattedText();
			text.Add(resultStatement.DescribeTrue(context.Language));

			var explanation = transitiveStatements == null
				? new Explanation(statement)
				: new Explanation(transitiveStatements);

			return new StatementAnswer(resultStatement, text, explanation);
		}

		protected override bool DoesStatementMatch(IQuestionProcessingContext<ComparisonQuestion> context, ComparisonStatement statement)
		{
			return	(statement.LeftValue == context.Question.LeftValue && statement.RightValue == context.Question.RightValue) ||
					(statement.RightValue == context.Question.LeftValue && statement.LeftValue == context.Question.RightValue);
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<ComparisonQuestion> context)
		{
			foreach (var statement in context.KnowledgeBase.Statements.Enumerate<ComparisonStatement>(context.ActiveContexts))
			{
				IConcept newLeftValue = null;
				if (statement.LeftValue == context.Question.LeftValue)
				{
					newLeftValue = statement.RightValue;
				}
				else if (statement.RightValue == context.Question.LeftValue)
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
						yield return new NestedQuestion(new ComparisonQuestion(newLeftValue, context.Question.RightValue), new IStatement[] { statement });
					}
				}
			}
		}

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, ICollection<ChildAnswer> childAnswers)
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
							new ComparisonStatement(context.Question.LeftValue, context.Question.RightValue, resultSign),
							context,
							transitiveStatements);
					}
				}
			}

			return Answer.CreateUnknown(context.Language);
		}
	}
}
