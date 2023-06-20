using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;

namespace AabSemantics.Test.Statements
{
	[TestFixture]
	public class SetModuleConsistencyCheckTest
	{
		private BooleanModule _booleanModule;
		private ClassificationModule _classificationModule;
		private SetModule _module;
		private ICollection<IExtensionModule> _modules;

		[OneTimeSetUp]
		public void RegisterModule()
		{
			_booleanModule = new BooleanModule();
			_classificationModule = new ClassificationModule();
			_module = new SetModule();

			_modules = new IExtensionModule[] { _booleanModule, _classificationModule, _module };

			foreach (var module in _modules)
			{
				module.RegisterMetadata();
			}
		}

#warning Revise SetModule.checkMultiValues method logic
		/*[Test]
		public void GivenManyValuesForTheSameSignWhenCheckMultiValuesThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default; 
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			var value1 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			var value2 = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value1);
			semanticNetwork.Concepts.Add(value2);
			semanticNetwork.DeclareThat(concept).HasSign(sign);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value1);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value2);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.Greater(result.Items.Count, 0);
		}*/

		[Test]
		public void GivenNoSignWhenCheckValuesWithoutSignThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.Greater(result.Items.Count, 0);
		}

		[Test]
		public void GivenCorrectSignsWithValuesWhenCheckValuesWithoutSignThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value);
			semanticNetwork.DeclareThat(concept).HasSign(sign);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.AreEqual(0, result.Items.Count);
		}

		[Test]
		public void GivenDuplicatingSignsWhenCheckSignDuplicationsThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(HasSignStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var child = ConceptCreationHelper.CreateConcept();
			var parent = ConceptCreationHelper.CreateConcept();
			var childSign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(child);
			semanticNetwork.Concepts.Add(parent);
			semanticNetwork.Concepts.Add(childSign);
			semanticNetwork.DeclareThat(child).IsDescendantOf(parent);
			semanticNetwork.DeclareThat(child).HasSign(childSign);
			semanticNetwork.DeclareThat(parent).HasSign(childSign);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.Greater(result.Items.Count, 0);
		}

		[Test]
		public void GivenNoDuplicatingSignsWhenCheckSignDuplicationsThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(HasSignStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var child = ConceptCreationHelper.CreateConcept();
			var parent = ConceptCreationHelper.CreateConcept();
			var childSign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			var parentSign = ConceptCreationHelper.CreateConcept().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(child);
			semanticNetwork.Concepts.Add(parent);
			semanticNetwork.Concepts.Add(childSign);
			semanticNetwork.Concepts.Add(parentSign);
			semanticNetwork.DeclareThat(child).IsDescendantOf(parent);
			semanticNetwork.DeclareThat(child).HasSign(childSign);
			semanticNetwork.DeclareThat(parent).HasSign(parentSign);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.AreEqual(0, result.Items.Count);
		}
	}
}
