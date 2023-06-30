using System.Globalization;
using System.Threading;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;

namespace AabSemantics.Tests.Localization
{
	[TestFixture]
	public class LanguagesExtensionsTest
	{
		[Test]
		public void GivenExistingCulture_WhenFindAppropriate_ThenReturnRequested()
		{
			// arrange
			Language searchedLanguage;
			var languages = new[]
			{
				new Language { Name = "English", Culture = "en-US" },
				searchedLanguage = new Language { Name = "Русский", Culture = "ru-RU" },
				new Language { Name = "Français", Culture = "fr-FR" },
			};

			var defaultLanguage = new Language { Name = "(default)", Culture = "#DEFAULT#" };

			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

			// act
			var foundLanguage = languages.FindAppropriate(defaultLanguage);

			// assert
			Assert.AreEqual(searchedLanguage, foundLanguage);
		}

		[Test]
		public void GivenMissingCulture_WhenFindAppropriate_ThenReturnDefault()
		{
			// arrange
			var languages = new[]
			{
				new Language { Name = "English", Culture = "en-US", },
				//new Language { Name = "Русский", Culture = "ru-RU", },
				new Language { Name = "Français", Culture = "fr-FR", },
			};

			var defaultLanguage = new Language { Name = "(default)", Culture = "#DEFAULT#" };

			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

			// act
			var foundLanguage = languages.FindAppropriate(defaultLanguage);

			// assert
			Assert.AreEqual(defaultLanguage, foundLanguage);
		}

		[Test]
		public void GivenPathWithoutModule_WhenGetBoundText_ThenReturnText()
		{
			// arrange
			var language = Language.Default;

			// act
			string text = language.GetBoundText("Attributes.None");

			// assert
			Assert.AreEqual(language.Attributes.None, text);
		}

		[Test]
		public void GivenPathWithModule_WhenGetBoundText_ThenReturnText()
		{
			// arrange
			Language.Default.Extensions.Add(LanguageBooleanModule.CreateDefault());

			var language = Language.Default;

			// act
			string text = language.GetBoundText("Boolean\\Attributes.IsBoolean");

			// assert
			Assert.AreEqual(language.GetExtension<ILanguageBooleanModule>().Attributes.IsBoolean, text);
		}

		[Test]
		public void GivenPathWithNullInMiddle_WhenGetBoundText_ThenReturnNull()
		{
			// arrange
			var language = new Language();

			// act
			string text = language.GetBoundText("Attributes.None");

			// assert
			Assert.IsNull(text);
		}
	}
}
