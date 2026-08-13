using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Answers;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Questions
{
	/// <summary>Asks how two processes relate in time.</summary>
	public class ProcessesQuestion : Question
	{
		#region Properties

		/// <summary>The first process; must carry the "is a process" attribute.</summary>
		public IConcept ProcessA
		{ get; set; }

		/// <summary>The second process; must carry the "is a process" attribute.</summary>
		public IConcept ProcessB
		{ get; set; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		/// <exception cref="System.ArgumentNullException">Either process is <c>null</c>.</exception>
		public ProcessesQuestion(IConcept processA, IConcept processB, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			ProcessA = processA.EnsureNotNull(nameof(processA));
			ProcessB = processB.EnsureNotNull(nameof(processB));
		}

		/// <summary>
		/// Finds the temporal relations between the two processes, normalising each matched statement
		/// to the asked operand order and recursing through the processes' ancestors when nothing matches.
		/// </summary>
		/// <param name="context">Context to search.</param>
		/// <returns>An answer listing the sequence signs, or the "unknown" answer.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<ProcessesQuestion, ProcessesStatement>()
				.WithTransitives(s => Task.FromResult(s.Count == 0), GetNestedQuestions)
				.Where(s => (s.ProcessA == ProcessA && s.ProcessB == ProcessB) || (s.ProcessB == ProcessA && s.ProcessA == ProcessB))
				.SelectCustomAsync(CreateAnswer)
				.ConfigureAwait(false);
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
