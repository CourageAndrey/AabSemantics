using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;
using System.Linq;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class ProcessesModuleConsistencyCheckTest
	{
		private ProcessesModule _module;

		[OneTimeSetUp]
		public void RegisterModule()
		{
			_module = new ProcessesModule();
			_module.RegisterMetadata();
		}

		[Test]
		public void GivenContradictions_WhenCheck_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ProcessesStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<ProcessesModule>();

			var concept1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsProcessAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsProcessAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).StartsAfterOtherFinished(concept2);
			semanticNetwork.DeclareThat(concept2).StartsAfterOtherFinished(concept1);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.Greater(result.Items.Count, 0);
			Assert.IsTrue(result.Items.Any(line => line.ToString().StartsWith("Impossible to detect sequence between ")));
		}

		[Test]
		public void GivenCorrectStatements_WhenCheck_ThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ProcessesStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<ProcessesModule>();

			var concept1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsProcessAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsProcessAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).SimultaneousWith(concept2);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.AreEqual(0, result.Items.Count);
		}
	}
}
