using System.Globalization;
using System.Threading;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Set.Localization;

namespace Inventor.Semantics.Test.Localization
{
	[TestFixture]
	public class LanguagesExtensionsTest
	{
		[Test]
		public void GivenExistingCultureWhenFindAppropriateThenReturnRequested()
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
		public void GivenMissingCultureWhenFindAppropriateThenReturnDefault()
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
		public void GivenPathWithoutModuleWhenGetBoundTextThenReturnText()
		{
			// arrange
			var language = Language.Default;

			// act
			string text = language.GetBoundText("Attributes.None");

			// assert
			Assert.AreEqual(language.Attributes.None, text);
		}

		[Test]
		public void GivenPathWithModuleWhenGetBoundTextThenReturnText()
		{
			// arrange
			Language.Default.Extensions.Add(LanguageSetModule.CreateDefault());

			var language = Language.Default;

			// act
			string text = language.GetBoundText("Set\\Attributes.IsSign");

			// assert
			Assert.AreEqual(language.GetExtension<ILanguageSetModule>().Attributes.IsSign, text);
		}

		[Test]
		public void GivenPathWithNullInMiddleWhenGetBoundTextThenReturnNull()
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
