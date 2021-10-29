using System.Collections.Generic;

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

		private static ILanguage createTestLanguage(int number)
		{
			return new Language
			{
				Name = $"Language {number}",
				Culture = $"c{number}",
			};
		}
	}
}
