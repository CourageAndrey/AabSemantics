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
						var sign = SequenceSigns.TryToCombineMutualSequences(transitiveStatement.SequenceSign, childStatement.SequenceSign);
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
	}
}
