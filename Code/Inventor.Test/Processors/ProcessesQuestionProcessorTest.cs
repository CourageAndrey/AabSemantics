using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Processors
{
	[TestFixture]
	public class ProcessesQuestionProcessorTest
	{
		[Test]
		[TestCaseSource(nameof(getAllValidCombinations))]
		public void CheckAllValidCombinations(IConcept signAI, IConcept signIB, IConcept resultSign)
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			IConcept processA, processB, processI;
			knowledgeBase.Concepts.Add(processA = createProcess("Process A"));
			knowledgeBase.Concepts.Add(processB = createProcess("Process B"));
			knowledgeBase.Concepts.Add(processI = createProcess("Process I"));

			var statementAI = new ProcessesStatement(processA, processI, signAI);
			var statementIA = new ProcessesStatement(processI, processA, signAI.Revert());
			var statementIB = new ProcessesStatement(processI, processB, signIB);
			var statementBI = new ProcessesStatement(processB, processI, signIB.Revert());

			foreach (var statementCombination in new[]
			{
				new Tuple<IStatement, IStatement>(statementAI, statementIB),
				new Tuple<IStatement, IStatement>(statementAI, statementBI),
				new Tuple<IStatement, IStatement>(statementIA, statementIB),
				new Tuple<IStatement, IStatement>(statementIA, statementBI),
			})
			{
				try
				{
					knowledgeBase.Statements.Add(statementCombination.Item1);
					knowledgeBase.Statements.Add(statementCombination.Item2);

					// act
					var question = new ProcessesQuestion(processA, processB);
					var answer = question.Ask(knowledgeBase.Context);

					// assert
					Assert.IsFalse(answer.IsEmpty);
					var processesAnswer = (StatementsAnswer<ProcessesStatement>) answer;

					var resultStatement = processesAnswer.Result.Single();
					Assert.AreSame(processA, resultStatement.ProcessA);
					Assert.AreSame(processB, resultStatement.ProcessB);
					Assert.AreSame(resultSign, resultStatement.SequenceSign);

					Assert.IsTrue(answer.Explanation.Statements.Contains(statementCombination.Item1));
					Assert.IsTrue(answer.Explanation.Statements.Contains(statementCombination.Item2));
				}
				finally
				{
					knowledgeBase.Statements.Remove(statementCombination.Item1);
					knowledgeBase.Statements.Remove(statementCombination.Item2);
				}
			}
		}

		[Test]
		public void CheckAllInvalidCombinations()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			IConcept processA, processB, processI;
			knowledgeBase.Concepts.Add(processA = createProcess("Process A"));
			knowledgeBase.Concepts.Add(processB = createProcess("Process B"));
			knowledgeBase.Concepts.Add(processI = createProcess("Process I"));

			var validCombinations = getAllValidCombinations().Select(array => new Tuple<IConcept, IConcept, IConcept>((IConcept) array[0], (IConcept) array[1], (IConcept) array[2])).ToList();

			foreach (var signAI in SystemConcepts.SequenceSigns)
			{
				foreach (var signIB in SystemConcepts.SequenceSigns)
				{
					if (!validCombinations.Any(c => c.Item1 == signAI && c.Item2 == signIB))
					{
						var statementAI = new ProcessesStatement(processA, processI, signAI);
						var statementIA = new ProcessesStatement(processI, processA, signAI.Revert());
						var statementIB = new ProcessesStatement(processI, processB, signIB);
						var statementBI = new ProcessesStatement(processB, processI, signIB.Revert());

						foreach (var statementCombination in new[]
						{
							new Tuple<IStatement, IStatement>(statementAI, statementIB),
							new Tuple<IStatement, IStatement>(statementAI, statementBI),
							new Tuple<IStatement, IStatement>(statementIA, statementIB),
							new Tuple<IStatement, IStatement>(statementIA, statementBI),
						})
						{
							try
							{
								knowledgeBase.Statements.Add(statementCombination.Item1);
								knowledgeBase.Statements.Add(statementCombination.Item2);

								// act
								var question = new ProcessesQuestion(processA, processB);
								var answer = question.Ask(knowledgeBase.Context);

								// assert
								Assert.IsTrue(answer.IsEmpty);
								Assert.AreEqual(0, answer.Explanation.Statements.Count);
							}
							finally
							{
								knowledgeBase.Statements.Remove(statementCombination.Item1);
								knowledgeBase.Statements.Remove(statementCombination.Item2);
							}
						}
					}
				}
			}
		}

		private static IConcept createProcess(string name)
		{
			var process = new Concept(new LocalizedStringConstant(l => name), new LocalizedStringConstant(l => name));
			process.Attributes.Add(IsProcessAttribute.Value);
			return process;
		}

		private static IEnumerable<object[]> getAllValidCombinations()
		{
			yield return new object[] { SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished };
			yield return new object[] { SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsAfterOtherFinished };

			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished };
			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsWhenOtherFinished };
			yield return new object[] { SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished };

			yield return new object[] { SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.StartsBeforeOtherFinished };
			yield return new object[] { SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished };

			yield return new object[] { SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished };
			yield return new object[] { SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished };

			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesWhenOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished };
			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished };
			yield return new object[] { SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };

			yield return new object[] { SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsWhenOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };

			yield return new object[] { SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished };
			yield return new object[] { SystemConcepts.StartsAfterOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsAfterOtherFinished };

			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.StartsAfterOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsWhenOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.StartsAfterOtherFinished };
			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsWhenOtherFinished };
			yield return new object[] { SystemConcepts.StartsWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished };

			yield return new object[] { SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.StartsBeforeOtherStarted };
			yield return new object[] { SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.StartsBeforeOtherFinished };
			yield return new object[] { SystemConcepts.StartsBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.StartsBeforeOtherFinished };

			yield return new object[] { SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished };
			yield return new object[] { SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished };

			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherStarted, SystemConcepts.FinishesAfterOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesWhenOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesAfterOtherFinished, SystemConcepts.FinishesAfterOtherFinished };
			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesWhenOtherFinished };
			yield return new object[] { SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };

			yield return new object[] { SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesWhenOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherStarted, SystemConcepts.FinishesBeforeOtherStarted };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesWhenOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };
			yield return new object[] { SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished, SystemConcepts.FinishesBeforeOtherFinished };

			yield return new object[] { SystemConcepts.Causes, SystemConcepts.Causes, SystemConcepts.Causes };
			yield return new object[] { SystemConcepts.IsCausedBy, SystemConcepts.IsCausedBy, SystemConcepts.IsCausedBy };
			yield return new object[] { SystemConcepts.SimultaneousWith, SystemConcepts.SimultaneousWith, SystemConcepts.SimultaneousWith };
		}
	}
}
