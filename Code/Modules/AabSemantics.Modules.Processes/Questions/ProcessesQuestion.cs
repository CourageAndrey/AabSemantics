using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Questions
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
			ProcessA = processA.EnsureNotNull(nameof(processA));
			ProcessB = processB.EnsureNotNull(nameof(processB));
		}

		/*public override IAnswer Process(IQuestionProcessingContext context)
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
		}*/

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var processesQuestionContext = (IQuestionProcessingContext<ProcessesQuestion>) context;
			(IConcept processAStart, IConcept processAEnd, IConcept processBStart, IConcept processBEnd) = PrepareTemporaryComparisons(processesQuestionContext);

			var startToStart = new ComparisonQuestion(processAStart, processBStart).Ask(context) as StatementAnswer;
			var endToEnd = new ComparisonQuestion(processAEnd, processBEnd).Ask(context) as StatementAnswer;
			var startToEnd = new ComparisonQuestion(processAStart, processBEnd).Ask(context) as StatementAnswer;
			var endToStart = new ComparisonQuestion(processAEnd, processBStart).Ask(context) as StatementAnswer;

			return CreateAnswer(
				processesQuestionContext,
				startToStart?.Result as ComparisonStatement,
				endToEnd?.Result as ComparisonStatement,
				startToEnd?.Result as ComparisonStatement,
				endToStart?.Result as ComparisonStatement);
		}

		private static (IConcept, IConcept, IConcept, IConcept) PrepareTemporaryComparisons(IQuestionProcessingContext<ProcessesQuestion> context)
		{
			IConcept processAStart = null, processAEnd = null, processBStart = null, processBEnd = null;
			var startPoints = new Dictionary<IConcept, IConcept>();
			var endPoints = new Dictionary<IConcept, IConcept>();

			(IConcept, IConcept) getPoints(IConcept process)
			{
				IConcept start, end;
				if (startPoints.TryGetValue(process, out start))
				{
					end = endPoints[process];
				}
				else
				{
					startPoints[process] = start = new Concept(null, LocalizedString.Empty, LocalizedString.Empty).WithAttribute(IsValueAttribute.Value);
					endPoints[process] = end = new Concept(null, LocalizedString.Empty, LocalizedString.Empty).WithAttribute(IsValueAttribute.Value);

					if (process == context.Question.ProcessA)
					{
						processAStart = start;
						processAEnd = end;
					}
					else if (process == context.Question.ProcessB)
					{
						processBStart = start;
						processBEnd = end;
					}

					context.SemanticNetwork.Statements.Add(new ComparisonStatement(null, start, end, ComparisonSigns.IsGreaterThanOrEqualTo)
					{
						Context = context
					});
				}

				return (start, end);
			}

			foreach (var processStatement in context.SemanticNetwork.Statements.Enumerate<ProcessesStatement>(context.ActiveContexts).ToList())
			{
				var (startA, endA) = getPoints(processStatement.ProcessA);
				var (startB, endB) = getPoints(processStatement.ProcessB);

				var comparisons = new List<ComparisonStatement>();

				if (processStatement.SequenceSign == SequenceSigns.StartsAfterOtherStarted)
				{
					comparisons.Add(new ComparisonStatement(null, startA, startB, ComparisonSigns.IsGreaterThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.StartsBeforeOtherStarted)
				{
					comparisons.Add(new ComparisonStatement(null, startA, startB, ComparisonSigns.IsLessThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.FinishesAfterOtherStarted)
				{
					comparisons.Add(new ComparisonStatement(null, endA, startB, ComparisonSigns.IsGreaterThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.FinishesWhenOtherStarted)
				{
					comparisons.Add(new ComparisonStatement(null, endA, startB, ComparisonSigns.IsEqualTo));
				}
				else if (processStatement.SequenceSign == SequenceSigns.FinishesBeforeOtherStarted)
				{
					comparisons.Add(new ComparisonStatement(null, endA, startB, ComparisonSigns.IsLessThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.StartsAfterOtherFinished)
				{
					comparisons.Add(new ComparisonStatement(null, startA, endB, ComparisonSigns.IsGreaterThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.StartsWhenOtherFinished)
				{
					comparisons.Add(new ComparisonStatement(null, startA, endB, ComparisonSigns.IsEqualTo));
				}
				else if (processStatement.SequenceSign == SequenceSigns.StartsBeforeOtherFinished)
				{
					comparisons.Add(new ComparisonStatement(null, startA, endB, ComparisonSigns.IsLessThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.FinishesAfterOtherFinished)
				{
					comparisons.Add(new ComparisonStatement(null, endA, endB, ComparisonSigns.IsGreaterThan));
				}
				else if (processStatement.SequenceSign == SequenceSigns.FinishesBeforeOtherFinished)
				{
					comparisons.Add(new ComparisonStatement(null, endA, endB, ComparisonSigns.IsLessThan));
				}

				foreach (var comparison in comparisons)
				{
					comparison.Context = context;
					context.SemanticNetwork.Statements.Add(comparison);
				}
			}

			return (processAStart, processAEnd, processBStart, processBEnd);
		}

		private IAnswer CreateAnswer(
			IQuestionProcessingContext<ProcessesQuestion> context,
			ComparisonStatement startToStart,
			ComparisonStatement endToEnd,
			ComparisonStatement startToEnd,
			ComparisonStatement endToStart)
		{
			var statements = new List<ProcessesStatement>();

			//	...

			return createAnswer(statements, context);
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
