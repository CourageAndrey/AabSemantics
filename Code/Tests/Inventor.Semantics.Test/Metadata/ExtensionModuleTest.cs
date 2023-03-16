using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Mathematics;
using Inventor.Semantics.Mathematics.Attributes;
using Inventor.Semantics.Mathematics.Localization;
using Inventor.Semantics.Mathematics.Questions;
using Inventor.Semantics.Mathematics.Statements;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Modules.Classification;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Modules.Classification.Questions;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Processes;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Processes.Localization;
using Inventor.Semantics.Processes.Questions;
using Inventor.Semantics.Processes.Statements;
using Inventor.Semantics.Set;
using Inventor.Semantics.Set.Attributes;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Questions;
using Inventor.Semantics.Set.Statements;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Text.Containers;

namespace Inventor.Semantics.Test.Metadata
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
		public void SucessfullyRegisterMetadata()
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
		public void RegisterMetadataOnce()
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
		public void AttachToSemanticNetworkOnce()
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
		public void ImpossibleToRegisterModuleWithUnregisteredDependencies()
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
		public void WithModulesDoesNotDependOnSequence()
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
		public void WithModulesFailsIfCannotResolveDependencies()
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
		public void WithModulesFailsOnCircularDependencies()
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
		public void ThereIsNoLanguageExtensionsByDefault()
		{
			// arrange
			var module = new TestModule("test");

			// act & assert
			Assert.AreEqual(0, module.GetLanguageExtensions().Count);
		}

		[Test]
		public void TestRegisteredMetadata()
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
			var attributeTypes = getAllAttributeTypes();
			Assert.AreEqual(Repositories.Attributes.Definitions.Count, getAllAttributeTypes().Count);
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
			var statementTypes = getAllStatementTypes();
			Assert.AreEqual(Repositories.Statements.Definitions.Count, statementTypes.Count);
			foreach (var type in statementTypes)
			{
				var definition = Repositories.Statements.Definitions[type];
				Assert.IsFalse(string.IsNullOrEmpty(definition.GetName(language)));
			}

			// assert questions
			var questionTypes = getAllQuestionsTypes();
			Assert.AreEqual(Repositories.Questions.Definitions.Count, questionTypes.Count);
			foreach (var type in questionTypes)
			{
				var definition = Repositories.Questions.Definitions[type];
				Assert.IsFalse(string.IsNullOrEmpty(definition.GetName(language)));
			}

			// assert answers
			var answerTypes = getAllAnswersTypes();
			Assert.AreEqual(Repositories.Answers.Definitions.Count, answerTypes.Count);
			foreach (var type in answerTypes)
			{
				var definition = Repositories.Answers.Definitions[type];
				Assert.AreSame(definition.Type, type);
			}
		}

		[Test]
		public void CheckLanguageExtensions()
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
		public void CheckCyclicClassifications()
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

			var text = TextRepresenters.PlainString.Represent(error, language).ToString();
			Assert.IsTrue(text.Contains("causes cyclic references"));
		}

		private static List<Type> getAllAttributeTypes()
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

		private static List<Type> getAllStatementTypes()
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

		private static List<Type> getAllQuestionsTypes()
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

		private static List<Type> getAllAnswersTypes()
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
