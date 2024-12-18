using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Localization;

namespace AabSemantics.Tests.Localization
{
	[TestFixture]
	public class LocalizedStringsTest
	{
		[Test]
		public void GivenConstantString_WhenGetValue_ThenReturnRequestedString()
		{
			// arrange
			var test = new LocalizedStringConstant(language => language.Name);
			var language1 = CreateTestLanguage(1);
			var language2 = CreateTestLanguage(2);

			// act
			string name1 = test.GetValue(language1);
			string name2 = test.GetValue(language2);

			// assert
			Assert.That(name1, Is.EqualTo("Language 1"));
			Assert.That(name2, Is.EqualTo("Language 2"));
		}

		[Test]
		public void GivenVariableString_WhenGetValue_ThenReturnRequestedString()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "c1", "Value 1" },
				{ "c2", "Value 2" },
			});
			var language1 = CreateTestLanguage(1);
			var language2 = CreateTestLanguage(2);

			// act
			string value1language = test.GetValue(language1);
			string value2language = test.GetValue(language2);
			string value1culture = test.GetValue(language1.Culture);
			string value2culture = test.GetValue(language2.Culture);

			// assert
			Assert.That(value1language, Is.EqualTo("Value 1"));
			Assert.That(value2language, Is.EqualTo("Value 2"));
			Assert.That(value1culture, Is.EqualTo("Value 1"));
			Assert.That(value2culture, Is.EqualTo("Value 2"));
		}

		[Test]
		public void GivenMissingLocale_WhenGetValue_ThenFail()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "c1", "Value 1" },
				{ "c2", "Value 2" },
			});
			var language = CreateTestLanguage(0);

			// act & assert
			var error = Assert.Throws<AbsentLocaleException>(() => { test.GetValue(language); });
			Assert.That(error.Locale, Is.EqualTo("c0"));
			error = Assert.Throws<AbsentLocaleException>(() => { test.GetValue(language.Culture); });
			Assert.That(error.Locale, Is.EqualTo("c0"));
		}

		[Test]
		public void GivenNoLocales_WhenGetValue_ThenDontFail()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>());
			var language = CreateTestLanguage(0);

			// act & assert
			Assert.DoesNotThrow(() => { test.GetValue(language); });
			Assert.DoesNotThrow(() => { test.GetValue(language.Culture); });
		}

		[Test]
		public void GivenDifferentLocalizedStrings_WhenToString_ThenReturnDebugConstantName()
		{
			// arrange
			var localizedStrings = new LocalizedString[]
			{
				new LocalizedStringConstant(l => "test"),
				new LocalizedStringVariable(),
			};

			// act & assert
			foreach (var localizedString in localizedStrings)
			{
				Assert.That(localizedString.ToString().Contains(Strings.TostringLocalized), Is.True);
			}
		}

		[Test]
		public void GivenDifferentLanguages_WhenGetValueFromEmptyLocalizedString_ThenReturnEmptyString()
		{
			// arrange
			var languages = new[]
			{
				new Language { Name = "English", Culture = "en-US" },
				new Language { Name = "Русский", Culture = "ru-RU" },
				new Language { Name = "Français", Culture = "fr-FR" },
			};

			// act && assert
			foreach (var language in languages)
			{
				Assert.That(LocalizedString.Empty.GetValue(language), Is.EqualTo(string.Empty));
			}
		}

		[Test]
		public void GivenLocalizedStringVariable_WhenRemoveLocale_ThenSucceed()
		{
			// arrange
			var s = new LocalizedStringVariable("en-US", "en-US");
			s.SetLocale("ru-RU", "ru-RU");
			s.SetLocale("fr-FR", "fr-FR");

			// assert before act
			Assert.That(s.GetValue("en-US"), Is.EqualTo("en-US"));
			Assert.That(s.GetValue("ru-RU"), Is.EqualTo("ru-RU"));
			Assert.That(s.GetValue("fr-FR"), Is.EqualTo("fr-FR"));

			// act
			s.RemoveLocale("ru-RU");

			// assert after act
			Assert.That(s.GetValue("en-US"), Is.EqualTo("en-US"));
			Assert.Throws<AbsentLocaleException>(() => s.GetValue("ru-RU"));
			Assert.That(s.GetValue("fr-FR"), Is.EqualTo("fr-FR"));
		}

		[Test]
		public void GivenNullString_WhenAsDictionary_ThenThrowException()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => LocalizedStringExtensions.AsDictionary(null));
		}

		[Test]
		public void GivenUnsupportedString_WhenAsDictionary_ThenThrowException()
		{
			// arrange
			var localizedString = new TestString();

			// act & assert
			Assert.Throws<NotSupportedException>(() => localizedString.AsDictionary());
		}

		[Test]
		public void GivenVariableString_WhenAsDictionary_ThenReturnAllLocales()
		{
			// arrange
			var values = new Dictionary<string, string>
			{
				{ "en-US", "dog" },
				{ "ru-RU", "собака" },
				{ "es-ES", "perro" },
				{ "de-DE", "hund" },
			};
			var localizedString = new LocalizedStringVariable(values);

			// act
			var result = localizedString.AsDictionary();

			// assert
			Assert.That(values.OrderBy(kvp => kvp.Key).SequenceEqual(result.OrderBy(kvp => kvp.Key)), Is.True);
		}

		[Test]
		public void GivenConstantString_WhenAsDictionary_ThenDefaultLocale()
		{
			// arrange
			var values = new Dictionary<string, string>
			{
				{ "en-US", "dog" },
				{ "ru-RU", "собака" },
				{ "es-ES", "perro" },
				{ "de-DE", "hund" },
			};
			var localizedString = new LocalizedStringConstant(language => values[language.Culture]);

			// act
			var result = localizedString.AsDictionary();

			// assert
			Assert.That(result[result.Keys.Single()], Is.EqualTo(values[Language.Default.Culture]));
		}

		private static ILanguage CreateTestLanguage(int number)
		{
			return new Language
			{
				Name = $"Language {number}",
				Culture = $"c{number}",
			};
		}

		private class TestString : ILocalizedString
		{
			public string GetValue(ILanguage language)
			{
				return language.Culture;
			}
		}
	}
}
