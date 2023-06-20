using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Processes;
using Inventor.Semantics.Modules.Processes.Attributes;
using Inventor.Semantics.Modules.Processes.Statements;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Text.Containers;

namespace Inventor.Semantics.Test.Statements
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
		public void GivenContradictionsWhenCheckThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ProcessesStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<ProcessesModule>();

			var concept1 = ConceptCreationHelper.CreateConcept().WithAttribute(IsProcessAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateConcept().WithAttribute(IsProcessAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).StartsAfterOtherFinished(concept2);
			semanticNetwork.DeclareThat(concept2).StartsAfterOtherFinished(concept1);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.Greater(result.Items.Count, 0);
		}

		[Test]
		public void GivenCorrectStatementsWhenCheckThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ProcessesStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModule<ProcessesModule>();

			var concept1 = ConceptCreationHelper.CreateConcept().WithAttribute(IsProcessAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateConcept().WithAttribute(IsProcessAttribute.Value);
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
