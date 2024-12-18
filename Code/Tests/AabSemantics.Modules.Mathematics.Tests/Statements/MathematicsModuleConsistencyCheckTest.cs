using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Metadata;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
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
		public void GivenContradictions_WhenCheck_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ComparisonStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(new IExtensionModule[] { _module });

			var concept1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).IsGreaterThan(concept2);
			semanticNetwork.DeclareThat(concept2).IsGreaterThan(concept1);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.That(result.Items.Count, Is.GreaterThan(0));
			Assert.That(result.Items.Any(line =>
				line.ToString().StartsWith("Impossible to compare") &&
				line.GetParameters().ContainsKey("#LEFTVALUE#") &&
				line.GetParameters().ContainsKey("#RIGHTVALUE#")), Is.True);
		}

		[Test]
		public void GivenCorrectStatements_WhenCheck_ThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(ComparisonStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(new IExtensionModule[] { _module });

			var concept1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			var concept2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);
			semanticNetwork.DeclareThat(concept1).IsGreaterThan(concept2);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.That(result.Items.Count, Is.EqualTo(0));
		}
	}
}
