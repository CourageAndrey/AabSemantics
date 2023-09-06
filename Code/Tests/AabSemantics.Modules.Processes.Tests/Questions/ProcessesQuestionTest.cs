using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Tests.Questions
{
	[TestFixture]
	public class ProcessesQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ProcessesQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new ProcessesQuestion(concept, null));
		}

		[Test]
		[TestCaseSource(nameof(GetSimpleStatements))]
		public void GivenSingleStatement_WhenAskProcessQuestion_ThenFindId(ProcessesStatement statement)
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			// act
			var answer = (StatementsAnswer<ProcessesStatement>) semanticNetwork.Supposing(new IStatement[] { statement }).Ask().WhatIsMutualSequenceOfProcesses(statement.ProcessA, statement.ProcessB);

			// assert
			Assert.IsFalse(answer.IsEmpty);
			Assert.IsTrue(answer.Result.Contains(statement));
			Assert.IsTrue(answer.Explanation.Statements.Contains(statement));
		}

		[Test]
		public void GivenWhatIsMutualSequenceOfProcesses_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			semanticNetwork.CreateProcessesTestData();

			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);
			semanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB);
			semanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB);

			// act
			var questionRegular = new ProcessesQuestion(processA, processB);
			var answerRegular = (StatementsAnswer<ProcessesStatement>) questionRegular.Ask(semanticNetwork.Context);

			var answerBuilder = (StatementsAnswer<ProcessesStatement>) semanticNetwork.Ask().WhatIsMutualSequenceOfProcesses(processA, processB);

			// assert
			Assert.IsTrue(answerRegular.Result.SequenceEqual(answerBuilder.Result));
			Assert.IsTrue(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements));
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var textRender = TextRenders.PlainString;

			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			language.Extensions.Add(LanguageProcessesModule.CreateDefault());

			var semanticNetwork = new SemanticNetwork(language);

			var question = new ProcessesQuestion(1.CreateConceptByObject(), 2.CreateConceptByObject());

			// act
			var answer = question.Ask(semanticNetwork.Context);
			var description = textRender.RenderText(answer.Description, language).ToString();

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.IsTrue(description.Contains(language.Questions.Answers.Unknown));
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		[TestCaseSource(nameof(GetAllValidCombinations))]
		public void GivenValidCombination_WhenBeingAsked_ThenReturnResult(IConcept signAI, IConcept signIB, IConcept resultSign)
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept processA, processB, processI;
			semanticNetwork.Concepts.Add(processA = CreateProcess("Process A"));
			semanticNetwork.Concepts.Add(processB = CreateProcess("Process B"));
			semanticNetwork.Concepts.Add(processI = CreateProcess("Process I"));

			var statementAI = new ProcessesStatement(null, processA, processI, signAI);
			var statementIA = new ProcessesStatement(null, processI, processA, SequenceSigns.Revert(signAI));
			var statementIB = new ProcessesStatement(null, processI, processB, signIB);
			var statementBI = new ProcessesStatement(null, processB, processI, SequenceSigns.Revert(signIB));

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

					Assert.Greater(processesAnswer.Result.Count, 0);
					Assert.IsTrue(processesAnswer.Result.All(s => s.ProcessA == processA));
					Assert.IsTrue(processesAnswer.Result.All(s => s.ProcessB == processB));
					Assert.IsTrue(processesAnswer.Result.Any(s => s.SequenceSign == resultSign));

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
		public void GivenInvalidCombination_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept processA, processB, processI;
			semanticNetwork.Concepts.Add(processA = CreateProcess("Process A"));
			semanticNetwork.Concepts.Add(processB = CreateProcess("Process B"));
			semanticNetwork.Concepts.Add(processI = CreateProcess("Process I"));

			var validCombinations = GetAllValidCombinations().Select(array => new Tuple<IConcept, IConcept, IConcept>((IConcept) array[0], (IConcept) array[1], (IConcept) array[2])).ToList();

			foreach (var signAI in SequenceSigns.All)
			{
				foreach (var signIB in SequenceSigns.All)
				{
					if (!validCombinations.Any(c => c.Item1 == signAI && c.Item2 == signIB))
					{
						var statementAI = new ProcessesStatement(null, processA, processI, signAI);
						var statementIA = new ProcessesStatement(null, processI, processA, SequenceSigns.Revert(signAI));
						var statementIB = new ProcessesStatement(null, processI, processB, signIB);
						var statementBI = new ProcessesStatement(null, processB, processI, SequenceSigns.Revert(signIB));

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

		private static IConcept CreateProcess(string name)
		{
			var process = name.CreateConceptByName();
			process.WithAttribute(IsProcessAttribute.Value);
			return process;
		}

		private static IEnumerable<object[]> GetAllValidCombinations()
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

		private static IEnumerable<object[]> GetSimpleStatements()
		{
			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);

			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			foreach (var sign in SequenceSigns.All)
			{
				yield return new object[] { new ProcessesStatement(null, processA, processB, sign) };
			}
		}
	}
}
