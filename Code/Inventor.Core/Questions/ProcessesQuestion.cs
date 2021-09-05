using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Concepts;
using Inventor.Core.Statements;
using Inventor.Core.Text.Containers;

namespace Inventor.Core.Questions
{
	public class ProcessesQuestion : Question
	{
		#region Properties

		public IConcept ProcessA
		{ get; set; }

		public IConcept ProcessB
		{ get; set; }

		#endregion

		public ProcessesQuestion(IConcept processA, IConcept processB, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));

			ProcessA = processA;
			ProcessB = processB;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<ProcessesQuestion, ProcessesStatement>()
				.WithTransitives(s => s.Count == 0, GetNestedQuestions)
				.Where(s => (s.ProcessA == ProcessA && s.ProcessB == ProcessB) || (s.ProcessB == ProcessA && s.ProcessA == ProcessB))
				.SelectCustom(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<ProcessesQuestion> context, ICollection<ProcessesStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			return statements.Count > 0
				? createAnswer(statements, context)
				: ProcessChildAnswers(context, childAnswers);
		}

		private static StatementsAnswer<ProcessesStatement> createAnswer(ICollection<ProcessesStatement> statements, IQuestionProcessingContext<ProcessesQuestion> context, ICollection<IStatement> transitiveStatements = null)
		{
			var resultStatements = new HashSet<ProcessesStatement>();
			var text = new UnstructuredContainer();

			foreach (var statement in statements)
			{
				var resultStatement = statement.SwapOperandsToMatchOrder(context.Question);
				if (resultStatements.All(s => s.SequenceSign != resultStatement.SequenceSign))
				{
					resultStatements.Add(resultStatement);
					text.Append(resultStatement.DescribeTrue());
				}
				addStatementConsequences(resultStatements, resultStatement, context);
			}

			var explanation = transitiveStatements == null
				? new Explanation(statements)
				: new Explanation(transitiveStatements);

			return new StatementsAnswer<ProcessesStatement>(resultStatements, text, explanation);
		}

		private static void addStatementConsequences(
			HashSet<ProcessesStatement> statements,
			ProcessesStatement newStatement,
			IQuestionProcessingContext<ProcessesQuestion> context)
		{
			foreach (var consequentSign in newStatement.SequenceSign.Consequently())
			{
				if (statements.All(s => s.SequenceSign != consequentSign))
				{
					statements.Add(new ProcessesStatement(null, context.Question.ProcessA, context.Question.ProcessB, consequentSign)
					{
						Context = context
					});
				}
			}
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<ProcessesQuestion> context)
		{
			var involvedValues = new HashSet<IConcept>(context.ActiveContexts
				.OfType<IQuestionProcessingContext<ProcessesQuestion>>()
				.Select(c => c.Question.ProcessA));

			var transitiveProcesses = new Dictionary<IConcept, ICollection<IStatement>>();
			foreach (var statement in context.SemanticNetwork.Statements.Enumerate<ProcessesStatement>(context.ActiveContexts))
			{
				IConcept newProcessA = null;
				if (statement.ProcessA == ProcessA)
				{
					newProcessA = statement.ProcessB;
				}
				else if (statement.ProcessB == ProcessA)
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
				var consequentStatements = new HashSet<ProcessesStatement>();
				foreach (var transitive in transitiveProcess.Value.OfType<ProcessesStatement>())
				{
					addStatementConsequences(consequentStatements, transitive, context);
				}

				yield return new NestedQuestion(new ProcessesQuestion(transitiveProcess.Key, ProcessB, consequentStatements), transitiveProcess.Value);
			}
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<ProcessesQuestion> context, ICollection<ChildAnswer> childAnswers)
		{
			foreach (var answer in childAnswers)
			{
				var childStatements = (answer.Answer as StatementsAnswer<ProcessesStatement>)?.Result ?? Array.Empty<ProcessesStatement>();
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
							resultStatements.Add(new ProcessesStatement(null, ProcessA,
								ProcessB,
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

			return Answer.CreateUnknown();
		}
	}
}
