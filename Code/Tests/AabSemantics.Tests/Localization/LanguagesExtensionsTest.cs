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
			Assert.That(foundLanguage, Is.EqualTo(searchedLanguage));
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
			Assert.That(foundLanguage, Is.EqualTo(defaultLanguage));
		}

		[Test]
		public void GivenPathWithoutModule_WhenGetBoundText_ThenReturnText()
		{
			// arrange
			var language = Language.Default;

			// act
			string text = language.GetBoundText("Attributes.None");

			// assert
			Assert.That(text, Is.EqualTo(language.Attributes.None));
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
			Assert.That(text, Is.EqualTo(language.GetExtension<ILanguageBooleanModule>().Attributes.IsBoolean));
		}

		/// <summary>
		/// An extension named without the <c>Language{Module}Module</c> pattern, like the WPF UI one.
		/// </summary>
		private class WpfUiModule : LanguageExtension
		{
			public string Caption
			{ get; set; }
		}

		[Test]
		public void GivenPathWithPlainlyNamedModule_WhenGetBoundText_ThenReturnText()
		{
			// arrange
			var extension = new WpfUiModule { Caption = "Semantic network" };

			var language = new Language();
			language.Extensions.Add(extension);

			// act
			string text = language.GetBoundText("WpfUiModule\\Caption");

			// assert
			Assert.That(text, Is.EqualTo(extension.Caption));
		}

		[Test]
		public void GivenPathWithNullInMiddle_WhenGetBoundText_ThenReturnNull()
		{
			// arrange
			var language = new Language();

			// act
			string text = language.GetBoundText("Attributes.None");

			// assert
			Assert.That(text, Is.Null);
		}

		[Test]
		public void GivenPathWithMissingProperty_WhenGetBoundText_ThenReturnNull()
		{
			// arrange
			var language = Language.Default;

			// act & assert
			Assert.That(language.GetBoundText("NoSuchProperty"), Is.Null);
			Assert.That(language.GetBoundText("Attributes.NoSuchProperty"), Is.Null);
		}
	}
}
