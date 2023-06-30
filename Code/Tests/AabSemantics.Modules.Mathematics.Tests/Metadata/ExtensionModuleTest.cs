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
			Assert.AreEqual(0, Repositories.Attributes.Definitions.Count);
			Assert.AreEqual(0, Repositories.Statements.Definitions.Count);
			Assert.AreEqual(0, Repositories.Questions.Definitions.Count);
			Assert.AreEqual(0, Repositories.Modules.Count);

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
			var systemConcepts = GetSystemConcepts();
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
			Assert.AreEqual(totalExtensionCount, language.Extensions.Count);
			Assert.IsNotNull(language.GetExtension<ILanguageBooleanModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageClassificationModule>());
			Assert.IsNotNull(language.GetExtension<ILanguageMathematicsModule>());
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
