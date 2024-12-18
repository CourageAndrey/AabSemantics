using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;

namespace AabSemantics.Tests.Localization
{
	[TestFixture]
	public class LanguageTest
	{
		[Test]
		public void GivenDefaultLanguage_WhenConvertToString_ThenReturnLanguageName()
		{
			// arrange
			var language = Language.Default;

			// assert
			Assert.That(language.ToString(), Is.EqualTo(language.Name));
		}

		[Test]
		public void GivenDefaultLanguageWithBaseModules_WhenCheckMembersTree_ThenAllMembersAreDefined()
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
		}
	}
}
