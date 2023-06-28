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
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Statements;
using AabSemantics.Test.Sample;
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
			Assert.IsTrue(module.AreAttributesRegistered);
			Assert.IsTrue(module.AreStatementsRegistered);
			Assert.IsTrue(module.AreQuestionsRegistered);
			Assert.IsTrue(module.AreAnswersRegistered);
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
			Assert.IsFalse(module.AreAttributesRegistered);
			Assert.IsFalse(module.AreStatementsRegistered);
			Assert.IsFalse(module.AreQuestionsRegistered);
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
			Assert.IsFalse(module.IsSemanticNetworkAttached);
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

			Assert.AreEqual(module.Name, error.ModuleName);
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
			Assert.AreEqual(expectedModules.Count, appliedModules.Count);
			Assert.IsFalse(expectedModules.Except(appliedModules).Any());
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
				Assert.AreEqual("2", error.ModuleName);
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
				Assert.IsTrue(error.ModuleName.Contains(module.Name));
			}
		}

		[Test]
		public void GivenDefaultLanguage_WhenGetExtensions_ThenReturnNothing()
		{
			// arrange
			var module = new TestModule("test");

			// act & assert
			Assert.AreEqual(0, module.GetLanguageExtensions().Count);
		}

		[Test]
		public void GivenCorrectModules_WhenRegisterMetadata_ThenSucceed()
		{
			// 0-assert
			Assert.AreEqual(0, Repositories.Attributes.Definitions.Count);
			Assert.AreEqual(0, Repositories.Statements.Definitions.Count);
			Assert.AreEqual(0, Repositories.Questions.Definitions.Count);
			Assert.AreEqual(0, Repositories.Modules.Count);

			// arrange & act
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new SetModule(),
				new MathematicsModule(),
				new ProcessesModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// assert modules
			Assert.AreEqual(modules.Length, Repositories.Modules.Count);
			foreach (var module in modules)
			{
				Assert.AreSame(module, Repositories.Modules[module.Name]);
			}

			// assert attributes
			var attributeTypes = GetAllAttributeTypes();
			Assert.AreEqual(Repositories.Attributes.Definitions.Count, GetAllAttributeTypes().Count);
			foreach (var type in attributeTypes)
			{
				var definition = Repositories.Attributes.Definitions[type];
				Assert.IsFalse(string.IsNullOrEmpty(definition.GetName(language)));
			}

			// assert concepts
			var semanticNetwork = new SemanticNetwork(Language.Default).WithModules(modules);
			var systemConcepts = SystemConcepts.GetAll().ToList();
			Assert.AreEqual(semanticNetwork.Concepts.Count, systemConcepts.Count);
			foreach (var concept in systemConcepts)
			{
				Assert.IsTrue(semanticNetwork.Concepts.Contains(concept));
			}

			// assert statements
			var statementTypes = GetAllStatementTypes();
			Assert.AreEqual(Repositories.Statements.Definitions.Count, statementTypes.Count);
			foreach (var type in statementTypes)
			{
				var definition = Repositories.Statements.Definitions[type];
				Assert.IsFalse(string.IsNullOrEmpty(definition.GetName(language)));
			}

			// assert questions
			var questionTypes = GetAllQuestionsTypes();
			Assert.AreEqual(Repositories.Questions.Definitions.Count, questionTypes.Count);
			foreach (var type in questionTypes)
			{
				var definition = Repositories.Questions.Definitions[type];
				Assert.IsFalse(string.IsNullOrEmpty(definition.GetName(language)));
			}

			// assert answers
			var answerTypes = GetAllAnswersTypes();
			Assert.AreEqual(Repositories.Answers.Definitions.Count, answerTypes.Count);
			foreach (var type in answerTypes)
			{
				var definition = Repositories.Answers.Definitions[type];
				Assert.AreSame(definition.Type, type);
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
				new SetModule(),
				new MathematicsModule(),
				new ProcessesModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// act
			int totalExtensionCount = modules.Sum(m => m.GetLanguageExtensions().Count);

			// assert
			Assert.AreEqual(totalExtensionCount, language.Extensions.Count);
			Assert.IsNotNull(language.GetExtension<ILanguageBooleanModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageClassificationModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageSetModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageMathematicsModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageProcessesModule>());
		}

		[Test]
		public void GivenDifferentInputs_WhenCheckCyclicClassifications_ThenAllWorksAsExpected()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = 1.CreateConcept();
			var concept2 = 2.CreateConcept();
			semanticNetwork.Concepts.Add(concept1);
			semanticNetwork.Concepts.Add(concept2);

			semanticNetwork.DeclareThat(concept1).IsAncestorOf(concept2);

			new ClassificationModule().RegisterMetadata();
			var isDefinition = Repositories.Statements.Definitions[typeof(IsStatement)];

			var validationResult = new UnstructuredContainer();

			// act correct
			isDefinition.CheckConsistency(semanticNetwork, validationResult);

			// assert correct
			Assert.AreEqual(0, validationResult.Items.Count);

			// arrange error
			semanticNetwork.DeclareThat(concept2).IsAncestorOf(concept1);

			// act error
			isDefinition.CheckConsistency(semanticNetwork, validationResult);

			// assert error
			var error = validationResult.Items.First();
			Assert.IsTrue(error.GetParameters().ContainsKey(Strings.ParamStatement));

			var text = TextRenders.PlainString.Render(error, language).ToString();
			Assert.IsTrue(text.Contains("causes cyclic references"));
		}

		private static List<Type> GetAllAttributeTypes()
		{
			return new List<Type>
			{
				typeof(IsValueAttribute),
				typeof(IsBooleanAttribute),
				typeof(IsSignAttribute),
				typeof(IsComparisonSignAttribute),
				typeof(IsProcessAttribute),
				typeof(IsSequenceSignAttribute),
			};
		}

		private static List<Type> GetAllStatementTypes()
		{
			return new List<Type>
			{
				typeof(IsStatement),
				typeof(HasPartStatement),
				typeof(GroupStatement),
				typeof(HasSignStatement),
				typeof(SignValueStatement),
				typeof(ComparisonStatement),
				typeof(ProcessesStatement),
			};
		}

		private static List<Type> GetAllQuestionsTypes()
		{
			return new List<Type>
			{
				typeof(CheckStatementQuestion),
				typeof(ComparisonQuestion),
				typeof(DescribeSubjectAreaQuestion),
				typeof(EnumerateAncestorsQuestion),
				typeof(EnumerateContainersQuestion),
				typeof(EnumerateDescendantsQuestion),
				typeof(EnumeratePartsQuestion),
				typeof(EnumerateSignsQuestion),
				typeof(FindSubjectAreaQuestion),
				typeof(GetCommonQuestion),
				typeof(GetDifferencesQuestion),
				typeof(HasSignQuestion),
				typeof(HasSignsQuestion),
				typeof(IsPartOfQuestion),
				typeof(IsQuestion),
				typeof(IsSignQuestion),
				typeof(IsSubjectAreaQuestion),
				typeof(IsValueQuestion),
				typeof(ProcessesQuestion),
				typeof(SignValueQuestion),
				typeof(WhatQuestion),
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
