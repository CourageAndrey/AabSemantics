using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Test.Localization
{
	[TestFixture]
	public class LocalizedStringsTest
	{
		[Test]
		public void TestConstantStrings()
		{
			// arrange
			var test = new LocalizedStringConstant(language => language.Name);
			var language1 = createTestLanguage(1);
			var language2 = createTestLanguage(2);

			// act
			string name1 = test.GetValue(language1);
			string name2 = test.GetValue(language2);

			// assert
			Assert.AreEqual("Language 1", name1);
			Assert.AreEqual("Language 2", name2);
		}

		[Test]
		public void TestVariableStrings()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "c1", "Value 1" },
				{ "c2", "Value 2" },
			});
			var language1 = createTestLanguage(1);
			var language2 = createTestLanguage(2);

			// act
			string value1language = test.GetValue(language1);
			string value2language = test.GetValue(language2);
			string value1culture = test.GetValue(language1.Culture);
			string value2culture = test.GetValue(language2.Culture);

			// assert
			Assert.AreEqual("Value 1", value1language);
			Assert.AreEqual("Value 2", value2language);
			Assert.AreEqual("Value 1", value1culture);
			Assert.AreEqual("Value 2", value2culture);
		}

		[Test]
		public void FailIfMissingLocale()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>
			{
				{ "c1", "Value 1" },
				{ "c2", "Value 2" },
			});
			var language = createTestLanguage(0);

			// act & assert
			var error = Assert.Throws<AbsentLocaleException>(() => { test.GetValue(language); });
			Assert.AreEqual("c0", error.Locale);
			error = Assert.Throws<AbsentLocaleException>(() => { test.GetValue(language.Culture); });
			Assert.AreEqual("c0", error.Locale);
		}

		[Test]
		public void DontFailIfNoLocales()
		{
			// arrange
			var test = new LocalizedStringVariable(new Dictionary<string, string>());
			var language = createTestLanguage(0);

			// act & assert
			Assert.DoesNotThrow(() => { test.GetValue(language); });
			Assert.DoesNotThrow(() => { test.GetValue(language.Culture); });
		}

		[Test]
		public void CheckToStrings()
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
				Assert.IsTrue(localizedString.ToString().Contains(Strings.TostringLocalized));
			}
		}

		[Test]
		public void CheckEmpty()
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
				Assert.AreEqual(string.Empty, LocalizedString.Empty.GetValue(language));
			}
		}

		[Test]
		public void CheckRemoveLocale()
		{
			// arrange
			var s = new LocalizedStringVariable("en-US", "en-US");
			s.SetLocale("ru-RU", "ru-RU");
			s.SetLocale("fr-FR", "fr-FR");

			// assert before act
			Assert.AreEqual("en-US", s.GetValue("en-US"));
			Assert.AreEqual("ru-RU", s.GetValue("ru-RU"));
			Assert.AreEqual("fr-FR", s.GetValue("fr-FR"));

			// act
			s.RemoveLocale("ru-RU");

			// assert after act
			Assert.AreEqual("en-US", s.GetValue("en-US"));
			Assert.Throws<AbsentLocaleException>(() => s.GetValue("ru-RU"));
			Assert.AreEqual("fr-FR", s.GetValue("fr-FR"));
		}

		[Test]
		public void GivenNullStringWhenAsDictionaryThenThrowException()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => LocalizedStringExtensions.AsDictionary(null));
		}

		[Test]
		public void GivenUnsupportedStringWhenAsDictionaryThenThrowException()
		{
			// arrange
			var localizedString = new TestString();

			// act & assert
			Assert.Throws<NotSupportedException>(() => localizedString.AsDictionary());
		}

		[Test]
		public void GivenVariableStringWhenAsDictionaryThenReturnAllLocales()
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
			Assert.IsTrue(values.OrderBy(kvp => kvp.Key).SequenceEqual(result.OrderBy(kvp => kvp.Key)));
		}

		[Test]
		public void GivenConstantStringWhenAsDictionaryThenDefaultLocale()
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
			Assert.AreEqual(values[Language.Default.Culture], result[result.Keys.Single()]);
		}

		private static ILanguage createTestLanguage(int number)
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
