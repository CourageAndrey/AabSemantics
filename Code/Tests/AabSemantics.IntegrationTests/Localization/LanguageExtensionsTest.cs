using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Set;
using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.IntegrationTests.Localization
{
	[TestFixture]
	public class LanguageExtensionsTest
	{
		[Test]
		public void TypedAndUntypedExtensionsWorkSimilar()
		{
			// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new MathematicsModule(),
				new ProcessesModule(),
				new SetModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;

			// act & assert
			Assert.That(
				language.GetAttributesExtension<ILanguageBooleanModule, Modules.Boolean.Localization.ILanguageAttributes>(),
				Is.SameAs(language.GetAttributesExtension<ILanguageBooleanModule>()));
			Assert.That(
				language.GetConceptsExtension<ILanguageBooleanModule, Modules.Boolean.Localization.ILanguageConcepts>(),
				Is.SameAs(language.GetConceptsExtension<ILanguageBooleanModule>()));
			Assert.That(
				language.GetQuestionsExtension<ILanguageBooleanModule, Modules.Boolean.Localization.ILanguageQuestions>(),
				Is.SameAs(language.GetQuestionsExtension<ILanguageBooleanModule>()));

			Assert.That(
				language.GetStatementsExtension<ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements>(),
				Is.SameAs(language.GetStatementsExtension<ILanguageClassificationModule>()));
			Assert.That(
				language.GetQuestionsExtension<ILanguageClassificationModule, Modules.Classification.Localization.ILanguageQuestions>(),
				Is.SameAs(language.GetQuestionsExtension<ILanguageClassificationModule>()));

			Assert.That(
				language.GetAttributesExtension<ILanguageMathematicsModule, Modules.Mathematics.Localization.ILanguageAttributes>(),
				Is.SameAs(language.GetAttributesExtension<ILanguageMathematicsModule>()));
			Assert.That(
				language.GetConceptsExtension<ILanguageMathematicsModule, Modules.Mathematics.Localization.ILanguageConcepts>(),
				Is.SameAs(language.GetConceptsExtension<ILanguageMathematicsModule>()));
			Assert.That(
				language.GetStatementsExtension<ILanguageMathematicsModule, Modules.Mathematics.Localization.ILanguageStatements>(),
				Is.SameAs(language.GetStatementsExtension<ILanguageMathematicsModule>()));
			Assert.That(
				language.GetQuestionsExtension<ILanguageMathematicsModule, Modules.Mathematics.Localization.ILanguageQuestions>(),
				Is.SameAs(language.GetQuestionsExtension<ILanguageMathematicsModule>()));

			Assert.That(
				language.GetAttributesExtension<ILanguageProcessesModule, Modules.Processes.Localization.ILanguageAttributes>(),
				Is.SameAs(language.GetAttributesExtension<ILanguageProcessesModule>()));
			Assert.That(
				language.GetConceptsExtension<ILanguageProcessesModule, Modules.Processes.Localization.ILanguageConcepts>(),
				Is.SameAs(language.GetConceptsExtension<ILanguageProcessesModule>()));
			Assert.That(
				language.GetStatementsExtension<ILanguageProcessesModule, Modules.Processes.Localization.ILanguageStatements>(),
				Is.SameAs(language.GetStatementsExtension<ILanguageProcessesModule>()));
			Assert.That(
				language.GetQuestionsExtension<ILanguageProcessesModule, Modules.Processes.Localization.ILanguageQuestions>(),
				Is.SameAs(language.GetQuestionsExtension<ILanguageProcessesModule>()));

			Assert.That(
				language.GetAttributesExtension<ILanguageSetModule, Modules.Set.Localization.ILanguageAttributes>(),
				Is.SameAs(language.GetAttributesExtension<ILanguageSetModule>()));
			Assert.That(
				language.GetStatementsExtension<ILanguageSetModule, Modules.Set.Localization.ILanguageStatements>(),
				Is.SameAs(language.GetStatementsExtension<ILanguageSetModule>()));
			Assert.That(
				language.GetQuestionsExtension<ILanguageSetModule, Modules.Set.Localization.ILanguageQuestions>(),
				Is.SameAs(language.GetQuestionsExtension<ILanguageSetModule>()));
		}
	}
}
