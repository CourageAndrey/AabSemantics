using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
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
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;

namespace AabSemantics.Modules.Processes.Tests.Metadata
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
				new ProcessesModule(),
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
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
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
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
			}

			// assert questions
			var questionTypes = GetAllQuestionsTypes();
			Assert.That(questionTypes.Count, Is.EqualTo(Repositories.Questions.Definitions.Count));
			foreach (var type in questionTypes)
			{
				var definition = Repositories.Questions.Definitions[type];
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
			}

			// assert answers
			var answerTypes = GetAllAnswersTypes();
			Assert.That(answerTypes.Count, Is.EqualTo(Repositories.Answers.Definitions.Count));
			foreach (var type in answerTypes)
			{
				var definition = Repositories.Answers.Definitions[type];
				Assert.That(definition.Type, Is.SameAs(type));
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
			Assert.That(totalExtensionCount, Is.EqualTo(language.Extensions.Count));
			Assert.That(language.GetExtension<ILanguageBooleanModule>(), Is.Not.Null);
			Assert.That(language.GetExtension<ILanguageClassificationModule>(), Is.Not.Null);
			Assert.That(language.GetExtension<ILanguageProcessesModule>(), Is.Not.Null);
		}

		private static List<Type> GetAllAttributeTypes()
		{
			return new List<Type>
			{
				typeof(IsValueAttribute),
				typeof(IsBooleanAttribute),
				typeof(IsProcessAttribute),
				typeof(IsSequenceSignAttribute),
			};
		}

		private static List<Type> GetAllStatementTypes()
		{
			return new List<Type>
			{
				typeof(IsStatement),
				typeof(ProcessesStatement),
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
				typeof(ProcessesQuestion),
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
			var list = new List<IConcept>(LogicalValues.All);
			list.AddRange(SequenceSigns.All);
			return list.ToHashSet();
		}
	}
}
