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
			Assert.IsNotNull(language);
			Assert.IsNotNull(language.Attributes);
			Assert.IsNotNull(language.Questions);
			Assert.IsNotNull(language.Questions.Answers);
			Assert.IsNotNull(language.Questions.Parameters);
			Assert.IsNotNull(language.Statements);
			Assert.IsNotNull(language.Statements.Consistency);

			var booleanExtension = language.GetExtension<ILanguageBooleanModule>();
			Assert.IsNotNull(booleanExtension.Attributes);
			Assert.IsNotNull(booleanExtension.Concepts);
			Assert.IsNotNull(booleanExtension.Questions);
			Assert.IsNotNull(booleanExtension.Questions.Names);
			Assert.IsNotNull(booleanExtension.Questions.Parameters);

			var classificationExtension = language.GetExtension<ILanguageClassificationModule>();
			Assert.IsNotNull(classificationExtension.Questions);
			Assert.IsNotNull(classificationExtension.Questions.Answers);
			Assert.IsNotNull(classificationExtension.Statements);
			Assert.IsNotNull(classificationExtension.Statements.Consistency);
			Assert.IsNotNull(classificationExtension.Statements.FalseFormatStrings);
			Assert.IsNotNull(classificationExtension.Statements.Hints);
			Assert.IsNotNull(classificationExtension.Statements.Names);
			Assert.IsNotNull(classificationExtension.Statements.QuestionFormatStrings);
			Assert.IsNotNull(classificationExtension.Statements.TrueFormatStrings);

			var processesExtension = language.GetExtension<ILanguageProcessesModule>();
			Assert.IsNotNull(processesExtension.Attributes);
			Assert.IsNotNull(processesExtension.Concepts);
			Assert.IsNotNull(processesExtension.Questions);
			Assert.IsNotNull(processesExtension.Questions.Parameters);
			Assert.IsNotNull(processesExtension.Questions.Parameters.ProcessA);
			Assert.IsNotNull(processesExtension.Questions.Parameters.ProcessB);
			Assert.IsNotNull(processesExtension.Statements);
			Assert.IsNotNull(processesExtension.Statements.Consistency);
			Assert.IsNotNull(processesExtension.Statements.FalseFormatStrings);
			Assert.IsNotNull(processesExtension.Statements.Hints);
			Assert.IsNotNull(processesExtension.Statements.Names);
			Assert.IsNotNull(processesExtension.Statements.QuestionFormatStrings);
			Assert.IsNotNull(processesExtension.Statements.TrueFormatStrings);
		}
	}
}
