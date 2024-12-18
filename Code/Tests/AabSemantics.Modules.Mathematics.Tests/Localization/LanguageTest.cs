using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Mathematics.Localization;

namespace AabSemantics.Modules.Mathematics.Tests.Localization
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
				new MathematicsModule(),
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

			var mathematicsExtension = language.GetExtension<ILanguageMathematicsModule>();
			Assert.That(mathematicsExtension.Attributes, Is.Not.Null);
			Assert.That(mathematicsExtension.Concepts, Is.Not.Null);
			Assert.That(mathematicsExtension.Questions, Is.Not.Null);
			Assert.That(mathematicsExtension.Questions.Parameters, Is.Not.Null);
			Assert.That(mathematicsExtension.Questions.Parameters.LeftValue, Is.Not.Null);
			Assert.That(mathematicsExtension.Questions.Parameters.RightValue, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.Consistency, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.FalseFormatStrings, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.Hints, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.Names, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.QuestionFormatStrings, Is.Not.Null);
			Assert.That(mathematicsExtension.Statements.TrueFormatStrings, Is.Not.Null);
		}
	}
}
