using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class StatementBuilderTest
	{
		[Test]
		public void GivenProcessesStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateEmptyConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			var statementsByConstructor = new List<ProcessesStatement>();
			var statementsByBuilder = new List<ProcessesStatement>();

			// act
			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.Causes));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.IsCausedBy));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.SimultaneousWith));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		[Test]
		public void GivenMultipleProcessesStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var processA = ConceptCreationHelper.CreateEmptyConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processesB = new[]
			{
				ConceptCreationHelper.CreateEmptyConcept(),
				ConceptCreationHelper.CreateEmptyConcept(),
				ConceptCreationHelper.CreateEmptyConcept(),
			};
			foreach (var processB in processesB)
			{
				processB.WithAttribute(IsProcessAttribute.Value);
			}

			var statementsByConstructor = new List<List<ProcessesStatement>>();
			var statementsByBuilder = new List<List<ProcessesStatement>>();

			// act
			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.Causes)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.IsCausedBy)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.SimultaneousWith)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processesB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				AssertAreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		private static void AssertAreEqual<T>(ICollection<T> sequence1, ICollection<T> sequence2)
			where T : IEquatable<T>
		{
			Assert.AreEqual(sequence1.Count, sequence1.Count);
			foreach (var item in sequence1)
			{
				Assert.IsTrue(sequence2.Contains(item));
			}
		}
	}
}
