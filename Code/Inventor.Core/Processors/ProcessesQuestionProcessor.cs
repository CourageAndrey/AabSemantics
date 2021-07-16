using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class ProcessesQuestionProcessor : QuestionProcessor<ProcessesQuestion, ProcessesStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<ProcessesQuestion> context, ICollection<ProcessesStatement> statements)
		{
			if (statements.Any())
			{
				return createAnswer(statements, context);
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static StatementsAnswer<ProcessesStatement> createAnswer(ICollection<ProcessesStatement> statements, IQuestionProcessingContext<ProcessesQuestion> context, ICollection<IStatement> transitiveStatements = null)
		{
			var resultStatements = new List<ProcessesStatement>();
			var text = new FormattedText();

			foreach (var statement in statements)
			{
				var resultStatement = statement.SwapOperandsToMatchOrder(context.Question);
				resultStatements.Add(resultStatement);
				text.Add(resultStatement.DescribeTrue(context.Language));
			}

			var explanation = transitiveStatements == null
				? new Explanation(statements)
				: new Explanation(transitiveStatements);

			return new StatementsAnswer<ProcessesStatement>(resultStatements, text, explanation);
		}

		protected override bool DoesStatementMatch(IQuestionProcessingContext<ProcessesQuestion> context, ProcessesStatement statement)
		{
			return	(statement.ProcessA == context.Question.ProcessA && statement.ProcessB == context.Question.ProcessB) ||
					(statement.ProcessB == context.Question.ProcessA && statement.ProcessA == context.Question.ProcessB);
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<ProcessesQuestion> context)
		{
			var involvedValues = new HashSet<IConcept>(context.ActiveContexts
				.OfType<IQuestionProcessingContext<ProcessesQuestion>>()
				.Select(c => c.Question.ProcessA));

			var transitiveProcesses = new Dictionary<IConcept, ICollection<IStatement>>();
			foreach (var statement in context.KnowledgeBase.Statements.Enumerate<ProcessesStatement>(context.ActiveContexts))
			{
				IConcept newProcessA = null;
				if (statement.ProcessA == context.Question.ProcessA)
				{
					newProcessA = statement.ProcessB;
				}
				else if (statement.ProcessB == context.Question.ProcessA)
				{
					newProcessA = statement.ProcessA;
				}

				if (newProcessA != null && !involvedValues.Contains(newProcessA))
				{
					ICollection<IStatement> transitiveStatements;
					if (!transitiveProcesses.TryGetValue(newProcessA, out transitiveStatements))
					{
						transitiveProcesses[newProcessA] = transitiveStatements = new List<IStatement>();
					}

					transitiveStatements.Add(statement);
				}
			}

			foreach (var transitiveProcess in transitiveProcesses)
			{
				yield return new NestedQuestion(new ProcessesQuestion(transitiveProcess.Key, context.Question.ProcessB), transitiveProcess.Value);
			}
		}

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<ProcessesQuestion> context, ICollection<ProcessesStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			foreach (var answer in childAnswers)
			{
				var childStatements = (answer.Answer as StatementsAnswer<ProcessesStatement>)?.Result ?? new ProcessesStatement[0];
				var resultStatements = new List<ProcessesStatement>();

				var transitiveStatements = answer.TransitiveStatements.OfType<ProcessesStatement>().ToList();
				var firstChild = childStatements.First();
				var firstTransitive = transitiveStatements.First();
				var intermediateValue = new[] { firstChild.ProcessA, firstChild.ProcessB }.Intersect(new[] { firstTransitive.ProcessA, firstTransitive.ProcessB }).Single();
				for (int i = 0; i < transitiveStatements.Count; i++)
				{
					if ((firstChild.ProcessA == intermediateValue) == (transitiveStatements[i].ProcessA == intermediateValue))
					{
						transitiveStatements[i] = transitiveStatements[i].SwapOperands();
					}
				}

				foreach (var childStatement in childStatements)
				{
					foreach (var transitiveStatement in transitiveStatements)
					{
						var sign = getSign(transitiveStatement.SequenceSign, childStatement.SequenceSign);
						if (sign != null)
						{
							resultStatements.Add(new ProcessesStatement(
								context.Question.ProcessA,
								context.Question.ProcessB,
								sign));
						}
					}
				}

				if (resultStatements.Count > 0)
				{
					var resultTransitiveStatements = new List<IStatement>(answer.TransitiveStatements);
					resultTransitiveStatements.AddRange(answer.Answer.Explanation.Statements);

					return createAnswer(resultStatements, context, resultTransitiveStatements);
				}
			}

			return Answer.CreateUnknown(context.Language);
		}

		internal static readonly IDictionary<IConcept, IDictionary<IConcept, IConcept>> ValidSequenceCombinations;

		private IConcept getSign(IConcept transitiveSign, IConcept childSign)
		{
			IDictionary<IConcept, IConcept> d;
			IConcept resultSign;
			return ValidSequenceCombinations.TryGetValue(transitiveSign, out d) && d.TryGetValue(childSign, out resultSign)
				? resultSign
				: null;
		}

		static ProcessesQuestionProcessor()
		{
			ValidSequenceCombinations = new Dictionary<IConcept, IDictionary<IConcept, IConcept>>();

			Action<IConcept, IConcept, IConcept> setValidCombination = (transitiveSign, childSign, resultSign) =>
			{
				IDictionary<IConcept, IConcept> d;
				if (!ValidSequenceCombinations.TryGetValue(transitiveSign, out d))
				{
					ValidSequenceCombinations[transitiveSign] = d = new Dictionary<IConcept, IConcept>();
				}
				d.Add(childSign, resultSign);
			};

			foreach (var combination in new[]
			{
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.Causes, SystemConcepts.Causes, SystemConcepts.Causes),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.IsCausedBy, SystemConcepts.IsCausedBy, SystemConcepts.IsCausedBy),
				new Tuple<IConcept, IConcept, IConcept>(SystemConcepts.SimultaneousWith, SystemConcepts.SimultaneousWith, SystemConcepts.SimultaneousWith),
			})
			{
				setValidCombination(combination.Item1, combination.Item2, combination.Item3);
			}
		}
	}
}
