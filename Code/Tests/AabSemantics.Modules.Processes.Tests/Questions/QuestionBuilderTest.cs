using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Tests.Questions
{
	[TestFixture]
	public class QuestionBuilderTest
	{
		[Test]
		public void GivenWhatIsMutualSequenceOfProcesses_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			semanticNetwork.CreateProcessesTestData();

			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateConcept();
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
	}
}
