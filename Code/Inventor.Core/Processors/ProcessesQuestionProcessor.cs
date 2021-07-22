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
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsAfterOtherFinished, SequenceSigns.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsAfterOtherFinished, SequenceSigns.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsAfterOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsWhenOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsAfterOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsWhenOtherFinished, SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.StartsBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesAfterOtherStarted, SequenceSigns.FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesAfterOtherFinished, SequenceSigns.FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesWhenOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherStarted, SequenceSigns.FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesWhenOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished, SequenceSigns.FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.Causes, SequenceSigns.Causes, SequenceSigns.Causes),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.IsCausedBy, SequenceSigns.IsCausedBy, SequenceSigns.IsCausedBy),
				new Tuple<IConcept, IConcept, IConcept>(SequenceSigns.SimultaneousWith, SequenceSigns.SimultaneousWith, SequenceSigns.SimultaneousWith),
			})
			{
				setValidCombination(combination.Item1, combination.Item2, combination.Item3);
			}
		}
	}
}
