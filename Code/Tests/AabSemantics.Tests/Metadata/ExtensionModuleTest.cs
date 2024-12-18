using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;
using AabSemantics.Text.Containers;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class ExtensionModuleTest
	{
		[SetUp, TearDown]
		public void ClearModules()
		{
			Language.Default.Extensions.Clear();
			Repositories.Modules.Clear();
			Repositories.Attributes.Definitions.Clear();
			Repositories.Statements.Definitions.Clear();
			Repositories.Questions.Definitions.Clear();
		}

		[Test]
		public void GivenCorrectModule_WhenRegisterMetadata_ThenSucceed()
		{
			// arrange
			var module = new TestModule("Test");

			// act
			module.RegisterMetadata();

			// assert
			Assert.That(module.AreAttributesRegistered, Is.True);
			Assert.That(module.AreStatementsRegistered, Is.True);
			Assert.That(module.AreQuestionsRegistered, Is.True);
			Assert.That(module.AreAnswersRegistered, Is.True);
		}

		[Test]
		public void GivenRegisteredModule_WhenTryToRegisterMetadata_ThenMetadataIsNotRegistered()
		{
			// arrange
			var module = new TestModule("Test");

			Repositories.Modules[module.Name] = module;

			// act
			module.RegisterMetadata();

			// assert
			Assert.That(module.AreAttributesRegistered, Is.False);
			Assert.That(module.AreStatementsRegistered, Is.False);
			Assert.That(module.AreQuestionsRegistered, Is.False);
		}

		[Test]
		public void GivenSemanticNetworkWithAttachedModule_WhenTryToAttachModuleWithTheSameName_ThenModuleIsNotAttached()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var module = new TestModule("Test");
			semanticNetwork.Modules[module.Name] = module;

			// act
			module.AttachTo(semanticNetwork);

			// assert
			Assert.That(module.IsSemanticNetworkAttached, Is.False);
		}

		[Test]
		public void GivenUnregisteredDependencies_WhenTryToRegisterModule_ThenFail()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var module = new TestModule("Test", new[] { "Dependency" });

			// act && assert
			var error = Assert.Throws<ModuleException>(() => module.AttachTo(semanticNetwork));

			Assert.That(error.ModuleName, Is.EqualTo(module.Name));
		}

		[Test]
		public void GivenManyDependentModules_WhenRegister_ThenAllDependenciesAreSuccessfullyResolved()
		{
			// arrange
			var language = Language.Default;
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language);

			var modules = new IExtensionModule[]
			{
				new TestModule("4", new string[] { "2", "3" }),
				new TestModule("5", new string[] { "1", "4" }),
				new TestModule("0"),
				new TestModule("2", new string[] { "1" }),
				new TestModule("3", new string[] { "1" }),
				new TestModule("1", new string[] { "0" }),
			};

			// act
			semanticNetwork = semanticNetwork.WithModules(modules);

			var appliedModules = semanticNetwork.Modules.Keys;
			var expectedModules = modules.Select(m => m.Name).ToList();

			// assert
			Assert.That(appliedModules.Count, Is.EqualTo(expectedModules.Count));
			Assert.That(expectedModules.Except(appliedModules).Any(), Is.False);
		}

		[Test]
		public void GivenUnresolvedDependencies_WhenTryToWithModules_ThenFail()
		{
			// arrange
			var language = Language.Default;
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language);

			var modules = new IExtensionModule[]
			{
				new TestModule("1"),
				new TestModule("2", new string[] { "3" }),
			};

			// act & assert
			var error = Assert.Throws<ModuleException>(() => semanticNetwork.WithModules(modules));

			foreach (var module in modules)
			{
				Assert.That(error.ModuleName, Is.EqualTo("2"));
			}
		}

		[Test]
		public void GivenCircularDependencies_WhenTryToWithModules_ThenFail()
		{
			// arrange
			var language = Language.Default;
			ISemanticNetwork semanticNetwork = new SemanticNetwork(language);

			var modules = new IExtensionModule[]
			{
				new TestModule("1", new string[] { "2" }),
				new TestModule("2", new string[] { "3" }),
				new TestModule("3", new string[] { "1" }),
			};

			// act & assert
			var error = Assert.Throws<ModuleException>(() => semanticNetwork.WithModules(modules));

			foreach (var module in modules)
			{
				Assert.That(error.ModuleName.Contains(module.Name), Is.True);
			}
		}

		[Test]
		public void GivenDefaultLanguage_WhenGetExtensions_ThenReturnNothing()
		{
			// arrange
			var module = new TestModule("test");

			// act & assert
			Assert.That(module.GetLanguageExtensions().Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenCorrectModules_WhenRegisterMetadata_ThenSucceed()
		{
			// 0-assert
			Assert.That(Repositories.Attributes.Definitions.Count, Is.EqualTo(0));
			Assert.That(Repositories.Statements.Definitions.Count, Is.EqualTo(0));
			Assert.That(Repositories.Questions.Definitions.Count, Is.EqualTo(0));
			Assert.That(Repositories.Modules.Count, Is.EqualTo(0));

			// arrange & act
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// assert modules
			Assert.That(Repositories.Modules.Count, Is.EqualTo(modules.Length));
			foreach (var module in modules)
			{
				Assert.That(Repositories.Modules[module.Name], Is.SameAs(module));
			}

			// assert attributes
			var attributeTypes = GetAllAttributeTypes();
			Assert.That(GetAllAttributeTypes().Count, Is.EqualTo(Repositories.Attributes.Definitions.Count));
			foreach (var type in attributeTypes)
			{
				var definition = Repositories.Attributes.Definitions[type];
				Assert.That(string.IsNullOrEmpty(definition.GetName(language)), Is.False);
			}

			// assert concepts
			var semanticNetwork = new SemanticNetwork(Language.Default).WithModules(modules);
			var systemConcepts = GetSystemConcepts();
			Assert.That(systemConcepts.Count, Is.EqualTo(semanticNetwork.Concepts.Count));
			foreach (var concept in systemConcepts)
			{
				Assert.That(semanticNetwork.Concepts.Contains(concept), Is.True);
			}

			// assert statements
			var statementTypes = GetAllStatementTypes();
			Assert.That(statementTypes.Count, Is.EqualTo(Repositories.Statements.Definitions.Count));
			foreach (var type in statementTypes)
			{
				var definition = Repositories.Statements.Definitions[type];
				Assert.That(string.IsNullOrEmpty(definition.GetName(language)), Is.False);
			}

			// assert questions
			var questionTypes = GetAllQuestionsTypes();
			Assert.That(questionTypes.Count, Is.EqualTo(Repositories.Questions.Definitions.Count));
			foreach (var type in questionTypes)
			{
				var definition = Repositories.Questions.Definitions[type];
				Assert.That(string.IsNullOrEmpty(definition.GetName(language)), Is.False);
			}

			// assert answers
			var answerTypes = GetAllAnswersTypes();
			Assert.That(answerTypes.Count, Is.EqualTo(Repositories.Answers.Definitions.Count));
			foreach (var type in answerTypes)
			{
				var definition = Repositories.Answers.Definitions[type];
				Assert.That(type, Is.SameAs(definition.Type));
			}
		}

		[Test]
		public void GivenLanguageExtensions_WhenGetThem_ThenAllAreGet()
		{
			// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// act
			int totalExtensionCount = modules.Sum(m => m.GetLanguageExtensions().Count);

			// assert
			Assert.That(language.Extensions.Count, Is.EqualTo(totalExtensionCount));
			Assert.That(language.GetExtension<ILanguageBooleanModule>(), Is.Not.Null);
			Assert.That(language.GetExtension<ILanguageClassificationModule>(), Is.Not.Null);
		}

		[Test]
		public void GivenDifferentInputs_WhenCheckCyclicClassifications_ThenAllWorksAsExpected()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = 1.CreateConceptByObject();
			var concept2 = 2.CreateConceptByObject();
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);

			semanticNetwork.DeclareThat(concept1).IsAncestorOf(concept2);

			new ClassificationModule().RegisterMetadata();
			var isDefinition = Repositories.Statements.Definitions[typeof(IsStatement)];

			var validationResult = new UnstructuredContainer();

			// act correct
			isDefinition.CheckConsistency(semanticNetwork, validationResult);

			// assert correct
			Assert.That(validationResult.Items.Count, Is.EqualTo(0));

			// arrange error
			semanticNetwork.DeclareThat(concept2).IsAncestorOf(concept1);

			// act error
			isDefinition.CheckConsistency(semanticNetwork, validationResult);

			// assert error
			var error = validationResult.Items.First();
			Assert.That(error.GetParameters().ContainsKey(Strings.ParamStatement), Is.True);

			var text = TextRenders.PlainString.Render(error, language).ToString();
			Assert.That(text.Contains("causes cyclic references"), Is.True);
		}

		private static List<Type> GetAllAttributeTypes()
		{
			return new List<Type>
			{
				typeof(IsValueAttribute),
				typeof(IsBooleanAttribute),
			};
		}

		private static List<Type> GetAllStatementTypes()
		{
			return new List<Type>
			{
				typeof(IsStatement),
			};
		}

		private static List<Type> GetAllQuestionsTypes()
		{
			return new List<Type>
			{
				typeof(CheckStatementQuestion),
				typeof(EnumerateAncestorsQuestion),
				typeof(EnumerateDescendantsQuestion),
				typeof(IsQuestion),
			};
		}

		private static List<Type> GetAllAnswersTypes()
		{
			return new List<Type>
			{
				typeof(Answer),
				typeof(BooleanAnswer),
				typeof(ConceptAnswer),
				typeof(ConceptsAnswer),
				typeof(StatementAnswer),
				typeof(StatementsAnswer),
			};
		}

		private static ICollection<IConcept> GetSystemConcepts()
		{
			return LogicalValues.All;
		}

		private class TestModule : ExtensionModule
		{
			public bool IsSemanticNetworkAttached
			{ get; set; }

			public bool AreAttributesRegistered
			{ get; set; }

			public bool AreStatementsRegistered
			{ get; set; }

			public bool AreQuestionsRegistered
			{ get; set; }

			public bool AreAnswersRegistered
			{ get; set; }

			public TestModule(string name, ICollection<string> dependencies = null)
				: base(name, dependencies ?? Array.Empty<string>())
			{ }

			protected override void Attach(ISemanticNetwork semanticNetwork)
			{
				base.Attach(semanticNetwork);
				IsSemanticNetworkAttached = true;
			}

			protected override void RegisterAttributes()
			{
				base.RegisterAttributes();
				AreAttributesRegistered = true;
			}

			protected override void RegisterStatements()
			{
				base.RegisterStatements();
				AreStatementsRegistered = true;
			}

			protected override void RegisterQuestions()
			{
				base.RegisterQuestions();
				AreQuestionsRegistered = true;
			}

			protected override void RegisterAnswers()
			{
				base.RegisterAnswers();
				AreAnswersRegistered = true;
			}
		}
	}
}
