using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Processes.Localization;

namespace AabSemantics.Modules.Processes.Tests.Localization
{
	[TestFixture]
	public class LanguageTest
	{
		[Test]
		public void GivenDefaultLanguageWithBaseModules_WhenCheckMembersTree_ThenAllMembersAreDefined()
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

			// assert
			Assert.That(language, Is.Not.Null);
			Assert.That(language.Attributes, Is.Not.Null);
			Assert.That(language.Questions, Is.Not.Null);
			Assert.That(language.Questions.Answers, Is.Not.Null);
			Assert.That(language.Questions.Parameters, Is.Not.Null);
			Assert.That(language.Statements, Is.Not.Null);
			Assert.That(language.Statements.Consistency, Is.Not.Null);

			var booleanExtension = language.GetExtension<ILanguageBooleanModule>();
			Assert.That(booleanExtension.Attributes, Is.Not.Null);
			Assert.That(booleanExtension.Concepts, Is.Not.Null);
			Assert.That(booleanExtension.Questions, Is.Not.Null);
			Assert.That(booleanExtension.Questions.Names, Is.Not.Null);
			Assert.That(booleanExtension.Questions.Parameters, Is.Not.Null);

			var classificationExtension = language.GetExtension<ILanguageClassificationModule>();
			Assert.That(classificationExtension.Questions, Is.Not.Null);
			Assert.That(classificationExtension.Questions.Answers, Is.Not.Null);
			Assert.That(classificationExtension.Statements, Is.Not.Null);
			Assert.That(classificationExtension.Statements.Consistency, Is.Not.Null);
			Assert.That(classificationExtension.Statements.FalseFormatStrings, Is.Not.Null);
			Assert.That(classificationExtension.Statements.Hints, Is.Not.Null);
			Assert.That(classificationExtension.Statements.Names, Is.Not.Null);
			Assert.That(classificationExtension.Statements.QuestionFormatStrings, Is.Not.Null);
			Assert.That(classificationExtension.Statements.TrueFormatStrings, Is.Not.Null);

			var processesExtension = language.GetExtension<ILanguageProcessesModule>();
			Assert.That(processesExtension.Attributes, Is.Not.Null);
			Assert.That(processesExtension.Concepts, Is.Not.Null);
			Assert.That(processesExtension.Questions, Is.Not.Null);
			Assert.That(processesExtension.Questions.Parameters, Is.Not.Null);
			Assert.That(processesExtension.Questions.Parameters.ProcessA, Is.Not.Null);
			Assert.That(processesExtension.Questions.Parameters.ProcessB, Is.Not.Null);
			Assert.That(processesExtension.Statements, Is.Not.Null);
			Assert.That(processesExtension.Statements.Consistency, Is.Not.Null);
			Assert.That(processesExtension.Statements.FalseFormatStrings, Is.Not.Null);
			Assert.That(processesExtension.Statements.Hints, Is.Not.Null);
			Assert.That(processesExtension.Statements.Names, Is.Not.Null);
			Assert.That(processesExtension.Statements.QuestionFormatStrings, Is.Not.Null);
			Assert.That(processesExtension.Statements.TrueFormatStrings, Is.Not.Null);
		}
	}
}
