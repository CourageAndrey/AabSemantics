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
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests.Metadata
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
				new MathematicsModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// assert modules
			Assert.That(modules.Length, Is.EqualTo(Repositories.Modules.Count));
			foreach (var module in modules)
			{
				Assert.That(module, Is.SameAs(Repositories.Modules[module.Name]));
			}

			// assert attributes
			var attributeTypes = GetAllAttributeTypes();
			Assert.That(Repositories.Attributes.Definitions.Count, Is.EqualTo(GetAllAttributeTypes().Count));
			foreach (var type in attributeTypes)
			{
				var definition = Repositories.Attributes.Definitions[type];
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
			}

			// assert concepts
			var semanticNetwork = new SemanticNetwork(Language.Default).WithModules(modules);
			var systemConcepts = GetSystemConcepts();
			Assert.That(semanticNetwork.Concepts.Count, Is.EqualTo(systemConcepts.Count));
			foreach (var concept in systemConcepts)
			{
				Assert.That(semanticNetwork.Concepts.Contains(concept), Is.True);
			}

			// assert statements
			var statementTypes = GetAllStatementTypes();
			Assert.That(Repositories.Statements.Definitions.Count, Is.EqualTo(statementTypes.Count));
			foreach (var type in statementTypes)
			{
				var definition = Repositories.Statements.Definitions[type];
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
			}

			// assert questions
			var questionTypes = GetAllQuestionsTypes();
			Assert.That(Repositories.Questions.Definitions.Count, Is.EqualTo(questionTypes.Count));
			foreach (var type in questionTypes)
			{
				var definition = Repositories.Questions.Definitions[type];
				Assert.That(definition.GetName(language), Is.Not.Null.Or.Empty);
			}

			// assert answers
			var answerTypes = GetAllAnswersTypes();
			Assert.That(Repositories.Answers.Definitions.Count, Is.EqualTo(answerTypes.Count));
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
				new MathematicsModule(),
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
			Assert.That(language.GetExtension<ILanguageMathematicsModule>(), Is.Not.Null);
		}

		private static List<Type> GetAllAttributeTypes()
		{
			return new List<Type>
			{
				typeof(IsValueAttribute),
				typeof(IsBooleanAttribute),
				typeof(IsComparisonSignAttribute),
			};
		}

		private static List<Type> GetAllStatementTypes()
		{
			return new List<Type>
			{
				typeof(IsStatement),
				typeof(ComparisonStatement),
			};
		}

		private static List<Type> GetAllQuestionsTypes()
		{
			return new List<Type>
			{
				typeof(CheckStatementQuestion),
				typeof(ComparisonQuestion),
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
			var list = new List<IConcept>(LogicalValues.All);
			list.AddRange(ComparisonSigns.All);
			return list.ToHashSet();
		}
	}
}
