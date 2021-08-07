﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class ComparisonQuestion : Question
	{
		#region Properties

		public IConcept LeftValue
		{ get; set; }

		public IConcept RightValue
		{ get; set; }

		#endregion

		public ComparisonQuestion(IConcept leftValue, IConcept rightValue, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));

			LeftValue = leftValue;
			RightValue = rightValue;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<ComparisonQuestion, ComparisonStatement>(s => (s.LeftValue == LeftValue && s.RightValue == RightValue) || (s.RightValue == LeftValue && s.LeftValue == RightValue))
				.ProcessTransitives(s => s.Count == 0, GetNestedQuestions)
				.Select(CreateAnswer)
				.Answer;
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			return statements.Count > 0
				? createAnswer(statements.First(), context)
				: ProcessChildAnswers(context, statements, childAnswers);
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

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, ICollection<ChildAnswer> childAnswers)
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
							new ComparisonStatement(LeftValue, RightValue, resultSign),
							context,
							transitiveStatements);
					}
				}
			}

			return Answer.CreateUnknown(context.Language);
		}
	}
}
