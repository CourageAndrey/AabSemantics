using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Set.Localization;
using ILanguageQuestions = AabSemantics.Modules.Set.Localization.ILanguageQuestions;

namespace AabSemantics.Modules.Set.Tests.Localization
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
				new SetModule(),
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

			var setExtension = language.GetExtension<ILanguageSetModule>();
			Assert.That(setExtension.Attributes, Is.Not.Null);
			Assert.That(setExtension.Questions, Is.Not.Null);
			Assert.That(setExtension.Questions.Answers, Is.Not.Null);
			Assert.That(setExtension.Questions.Parameters, Is.Not.Null);
			Assert.That(setExtension.Statements, Is.Not.Null);
			Assert.That(setExtension.Statements.Consistency, Is.Not.Null);
			Assert.That(setExtension.Statements.FalseFormatStrings, Is.Not.Null);
			Assert.That(setExtension.Statements.Hints, Is.Not.Null);
			Assert.That(setExtension.Statements.Names, Is.Not.Null);
			Assert.That(setExtension.Statements.QuestionFormatStrings, Is.Not.Null);
			Assert.That(setExtension.Statements.TrueFormatStrings, Is.Not.Null);
		}

		[Test]
		public void GivenLanguageQuestionParameters_WhenUse_ThenAreUsed()
		{
			// arrange
			var modules = new IExtensionModule[]
			{
				new BooleanModule(),
				new ClassificationModule(),
				new SetModule(),
			};
			foreach (var module in modules)
			{
				module.RegisterMetadata();
			}

			var language = Language.Default;
			var lang = language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Parameters;

			// assert
			Assert.That(lang.Sign, Is.EqualTo("SIGN"));
			Assert.That(lang.Area, Is.EqualTo("SUBJECT_AREA"));
			Assert.That(lang.Concept1, Is.EqualTo("CONCEPT 1"));
			Assert.That(lang.Concept2, Is.EqualTo("CONCEPT 2"));
		}
	}
}
