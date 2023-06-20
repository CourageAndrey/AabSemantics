using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Mathematics;
using Inventor.Semantics.Modules.Mathematics.Statements;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Text.Containers;

namespace Inventor.Semantics.Test.Statements
{
	[TestFixture]
	public class MathematicsModuleConsistencyCheckTest
	{
		private MathematicsModule _module;

		[OneTimeSetUp]
		public void RegisterModule()
		{
			_module = new MathematicsModule();
			_module.RegisterMetadata();
		}

		[Test]
		public void GivenContradictionsWhenCheckThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ComparisonStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(new IExtensionModule[] { _module });

			var concept1 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).IsGreaterThan(concept2);
			semanticNetwork.DeclareThat(concept2).IsGreaterThan(concept1);

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
			var statementDefinition = Repositories.Statements.Definitions[typeof(ComparisonStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(new IExtensionModule[] { _module });

			var concept1 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).IsGreaterThan(concept2);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.AreEqual(0, result.Items.Count);
		}
	}
}
