using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class ExtensionModuleTest
	{
		[SetUp, TearDown]
		public void ClearModules()
		{
			Repositories.Modules.Clear();
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
		}
	}
}
