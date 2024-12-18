using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;

namespace AabSemantics.Modules.Set.Tests.Statements
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

		[Test]
		public void GivenMultipleValuesOnTheSameLevel_WhenCheckMultiValues_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default; 
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			var value1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			var value2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value1);
			semanticNetwork.Concepts.Add(value2);
			semanticNetwork.DeclareThat(concept).HasSign(sign);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value1);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value2);

			var result = new UnstructuredContainer();

			var render = TextRenders.PlainString;

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);
			var text = render.RenderText(result, language).ToString();

			// assert
			Assert.That(result.Items.Count, Is.GreaterThan(0));
			Assert.That(text.Contains(" concept is uncertain, because its value set multiple times."), Is.True);
		}

		[Test]
		public void GivenMultipleValuesOnPreviousLevels_WhenCheckMultiValues_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default; 
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var parent1 = ConceptCreationHelper.CreateEmptyConcept();
			var parent2 = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			var value1 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			var value2 = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(parent1);
			semanticNetwork.Concepts.Add(parent2);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value1);
			semanticNetwork.Concepts.Add(value2);
			semanticNetwork.DeclareThat(parent1).HasSign(sign);
			semanticNetwork.DeclareThat(parent2).HasSign(sign);
			semanticNetwork.DeclareThat(concept).IsDescendantOf(parent1);
			semanticNetwork.DeclareThat(concept).IsDescendantOf(parent2);
			semanticNetwork.DeclareThat(parent1).HasSignValue(sign, value1);
			semanticNetwork.DeclareThat(parent2).HasSignValue(sign, value2);

			var result = new UnstructuredContainer();

			var render = TextRenders.PlainString;

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);
			var text = render.RenderText(result, language).ToString();

			// assert
			Assert.That(result.Items.Count, Is.GreaterThan(0));
			Assert.That(text.Contains(" concept is uncertain, because many ancestors define their own values."), Is.True);
		}

		[Test]
		public void GivenNoSign_WhenCheckValuesWithoutSign_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);

			var result = new UnstructuredContainer();

			var render = TextRenders.PlainString;

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);
			var text = render.RenderText(result, language).ToString();

			// assert
			Assert.That(result.Items.Count, Is.GreaterThan(0));
			Assert.That(text.Contains(" defines value of sign, which does not belong to concept."), Is.True);
		}

		[Test]
		public void GivenCorrectSignsWithValues_WhenCheckValuesWithoutSign_ThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(SignValueStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var concept = ConceptCreationHelper.CreateEmptyConcept();
			var sign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsValueAttribute.Value);
			semanticNetwork.Concepts.Add(concept);
			semanticNetwork.Concepts.Add(sign);
			semanticNetwork.Concepts.Add(value);
			semanticNetwork.DeclareThat(concept).HasSign(sign);
			semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);

			var result = new UnstructuredContainer();

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);

			// assert
			Assert.That(result.Items.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenDuplicatingSigns_WhenCheckSignDuplications_ThenFail()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(HasSignStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var child = ConceptCreationHelper.CreateEmptyConcept();
			var parent = ConceptCreationHelper.CreateEmptyConcept();
			var childSign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			semanticNetwork.Concepts.Add(child);
			semanticNetwork.Concepts.Add(parent);
			semanticNetwork.Concepts.Add(childSign);
			semanticNetwork.DeclareThat(child).IsDescendantOf(parent);
			semanticNetwork.DeclareThat(child).HasSign(childSign);
			semanticNetwork.DeclareThat(parent).HasSign(childSign);

			var result = new UnstructuredContainer();

			var render = TextRenders.PlainString;

			// act
			statementDefinition.CheckConsistency(semanticNetwork, result);
			var text = render.RenderText(result, language).ToString();

			// assert
			Assert.That(result.Items.Count, Is.GreaterThan(0));
			Assert.That(text.Contains(" cause sign value overload."), Is.True);
		}

		[Test]
		public void GivenNoDuplicatingSigns_WhenCheckSignDuplications_ThenSucceed()
		{
			// arrange
			var statementDefinition = Repositories.Statements.Definitions[typeof(HasSignStatement)];

			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language)
				.WithModules(_modules);

			var child = ConceptCreationHelper.CreateEmptyConcept();
			var parent = ConceptCreationHelper.CreateEmptyConcept();
			var childSign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
			var parentSign = ConceptCreationHelper.CreateEmptyConcept().WithAttribute(IsSignAttribute.Value);
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
			Assert.That(result.Items.Count, Is.EqualTo(0));
		}
	}
}
