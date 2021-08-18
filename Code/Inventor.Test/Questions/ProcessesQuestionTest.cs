using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class ProcessesQuestionTest
	{
		[Test]
		[TestCaseSource(nameof(getAllValidCombinations))]
		public void CheckAllValidCombinations(IConcept signAI, IConcept signIB, IConcept resultSign)
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept processA, processB, processI;
			semanticNetwork.Concepts.Add(processA = createProcess("Process A"));
			semanticNetwork.Concepts.Add(processB = createProcess("Process B"));
			semanticNetwork.Concepts.Add(processI = createProcess("Process I"));

			var statementAI = new ProcessesStatement(processA, processI, signAI);
			var statementIA = new ProcessesStatement(processI, processA, SequenceSigns.Revert(signAI));
			var statementIB = new ProcessesStatement(processI, processB, signIB);
			var statementBI = new ProcessesStatement(processB, processI, SequenceSigns.Revert(signIB));

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
					semanticNetwork.Statements.Add(statementCombination.Item1);
					semanticNetwork.Statements.Add(statementCombination.Item2);

					// act
					var answer = semanticNetwork.Ask().WhatIsMutualSequenceOfProcesses(processA, processB);

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
					semanticNetwork.Statements.Remove(statementCombination.Item1);
					semanticNetwork.Statements.Remove(statementCombination.Item2);
				}
			}
		}

		[Test]
		public void CheckAllInvalidCombinations()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept processA, processB, processI;
			semanticNetwork.Concepts.Add(processA = createProcess("Process A"));
			semanticNetwork.Concepts.Add(processB = createProcess("Process B"));
			semanticNetwork.Concepts.Add(processI = createProcess("Process I"));

			var validCombinations = getAllValidCombinations().Select(array => new Tuple<IConcept, IConcept, IConcept>((IConcept) array[0], (IConcept) array[1], (IConcept) array[2])).ToList();

			foreach (var signAI in SequenceSigns.All)
			{
				foreach (var signIB in SequenceSigns.All)
				{
					if (!validCombinations.Any(c => c.Item1 == signAI && c.Item2 == signIB))
					{
						var statementAI = new ProcessesStatement(processA, processI, signAI);
						var statementIA = new ProcessesStatement(processI, processA, SequenceSigns.Revert(signAI));
						var statementIB = new ProcessesStatement(processI, processB, signIB);
						var statementBI = new ProcessesStatement(processB, processI, SequenceSigns.Revert(signIB));

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
								semanticNetwork.Statements.Add(statementCombination.Item1);
								semanticNetwork.Statements.Add(statementCombination.Item2);

								// act
								var answer = semanticNetwork.Ask().WhatIsMutualSequenceOfProcesses(processA, processB);

								// assert
								Assert.IsTrue(answer.IsEmpty);
								Assert.AreEqual(0, answer.Explanation.Statements.Count);
							}
							finally
							{
								semanticNetwork.Statements.Remove(statementCombination.Item1);
								semanticNetwork.Statements.Remove(statementCombination.Item2);
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
			foreach (var combinations in SequenceSigns.ValidSequenceCombinations)
			{
				var transitiveSign = combinations.Key;
				foreach (var combination in combinations.Value)
				{
					var childSign = combination.Key;
					var expectedResultSign = combination.Value;
					yield return new object[] { transitiveSign, childSign, expectedResultSign };
				}
			}
		}
	}
}
