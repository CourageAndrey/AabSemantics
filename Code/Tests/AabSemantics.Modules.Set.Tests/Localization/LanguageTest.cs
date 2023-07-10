using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Set.Localization;

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

			var setExtension = language.GetExtension<ILanguageSetModule>();
			Assert.IsNotNull(setExtension.Attributes);
			Assert.IsNotNull(setExtension.Questions);
			Assert.IsNotNull(setExtension.Questions.Answers);
			Assert.IsNotNull(setExtension.Questions.Parameters);
			Assert.IsNotNull(setExtension.Statements);
			Assert.IsNotNull(setExtension.Statements.Consistency);
			Assert.IsNotNull(setExtension.Statements.FalseFormatStrings);
			Assert.IsNotNull(setExtension.Statements.Hints);
			Assert.IsNotNull(setExtension.Statements.Names);
			Assert.IsNotNull(setExtension.Statements.QuestionFormatStrings);
			Assert.IsNotNull(setExtension.Statements.TrueFormatStrings);
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
			var lang = language.GetExtension<ILanguageSetModule>().Questions.Parameters;

			// assert
			Assert.AreEqual("SIGN", lang.Sign);
			Assert.AreEqual("SUBJECT_AREA", lang.Area);
			Assert.AreEqual("CONCEPT 1", lang.Concept1);
			Assert.AreEqual("CONCEPT 2", lang.Concept2);
		}
	}
}
