using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Tests.Localization
{
	[TestFixture]
	public class LanguageSerializationTest
	{
		[TearDown]
		public void RestoreDefaultLanguage()
		{
			Language.Default.Extensions.Clear();
			Repositories.Modules.Clear();
			XmlHelper.ResetCache();
		}

		[Test]
		public void GivenDefaultLanguage_WhenSerializeAndDeserializeBack_ThenGetTheSameOne()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			// act
			Language restored;
			try
			{
				language.SerializeToXmlFile(testFileName);
				restored = testFileName.DeserializeFromXmlFile<Language>();
			}
			finally
			{
				if (File.Exists(testFileName))
				{
					File.Delete(testFileName);
				}
			}

			// assert
			Assert.That(restored.FileName, Is.Null);
			AssertLanguagesAreEqual(language, restored);
		}

		[Test]
		public void GivenUnregisteredExtension_WhenTryToSerializeLanguage_ThenFail()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			language.Extensions.Add(LanguageClassificationModule.CreateDefault());

			// act
			var serializationError = Assert.Throws<InvalidOperationException>(() => { language.SerializeToXmlFile(testFileName); });

			// assert
			Assert.That(serializationError.Message.Contains("XML"), Is.True);

			string serializationErrorDetails = serializationError.InnerException.Message;
			Assert.That(serializationErrorDetails.Contains(typeof(XmlIncludeAttribute).Name.Replace("Attribute", string.Empty)), Is.True);
		}

		[Test]
		public void GivenRegisteredExtension_WhenSerializeLanguage_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();

			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());

			Language.PrepareModulesToSerialization<Language>();

			// act
			Language restored;
			try
			{
				language.SerializeToXmlFile(testFileName);
				restored = testFileName.DeserializeFromXmlFile<Language>();
			}
			finally
			{
				if (File.Exists(testFileName))
				{
					File.Delete(testFileName);
				}
			}

			// assert
			Assert.That(restored.FileName, Is.Null);
			AssertLanguagesAreEqual(language, restored);
		}

		private static void AssertLanguagesAreEqual(Language language, Language restored)
		{
			Assert.That(restored.FileName, Is.Null);
			Assert.That(restored.Culture, Is.EqualTo(language.Culture));
			Assert.That(restored.Name, Is.EqualTo(language.Name));

			language.Attributes.AssertPropertiesAreEqual(restored.Attributes);
			language.Statements.AssertPropertiesAreEqual(restored.Statements);
			language.Questions.AssertPropertiesAreEqual(restored.Questions);

			Assert.That(restored.Extensions.Count, Is.EqualTo(language.Extensions.Count));
			foreach (var extension in language.Extensions)
			{
				var extensionType = extension.GetType();
				var restoredExtension = restored.Extensions.First(e => e.GetType() == extensionType);
				extension.AssertPropertiesAreEqual(restoredExtension);
			}
		}

		[Test]
		public void GivenAdditionalLanguages_WhenDeserializeThem_ThenSucceed()
		{
			// arrange
			string tempFolder = Path.ChangeExtension(Path.GetTempFileName(), string.Empty);
			var additionalLanguages = new[]
			{
				CreateTestLanguage("Deutsch", "de"),
				CreateTestLanguage("Français", "fr"),
				CreateTestLanguage("Español", "es"),
				CreateTestLanguage("Ελληνικά", "el"),
				CreateTestLanguage("فارسی", "fa"),
				CreateTestLanguage("ქართული", "ka"),
				CreateTestLanguage("中文", "zh"),
				CreateTestLanguage("日本語", "ja"),
			};
			var languagesDictionary = additionalLanguages.OrderBy(l => l.Culture).ToDictionary(l => l.Culture, l => l.Name);

			try
			{
				Directory.CreateDirectory(Path.Combine(tempFolder, LanguagesExtensions.DefaultFolderPath));
				foreach (var language in additionalLanguages)
				{
					language.SerializeToXmlFile(Path.Combine(tempFolder, LanguagesExtensions.DefaultFolderPath, $"{language.Name}.xml"));
				}

				// act
				var deserialized = tempFolder.LoadAdditionalLanguages();
				var deserializedDictionary = deserialized.OrderBy(l => l.Culture).ToDictionary(l => l.Culture, l => l.Name);

				// assert
				Assert.That(languagesDictionary.SequenceEqual(deserializedDictionary), Is.True);
			}
			finally
			{
				if (Directory.Exists(tempFolder))
				{
					Directory.Delete(tempFolder, true);
				}
			}
		}

		[Test]
		public void GivenNoLanguagesFolder_WhenTryToLoadFromIt_ThenCreateIt()
		{
			// arrange
			string tempFolder = Path.ChangeExtension(Path.GetTempFileName(), string.Empty);

			try
			{
				Directory.CreateDirectory(Path.Combine(tempFolder));

				// act
				tempFolder.LoadAdditionalLanguages();

				// assert
				Assert.That(Directory.Exists(Path.Combine(tempFolder, LanguagesExtensions.DefaultFolderPath)), Is.True);
			}
			finally
			{
				if (Directory.Exists(tempFolder))
				{
					Directory.Delete(tempFolder, true);
				}
			}
		}

		private static Language CreateTestLanguage(string name, string culture)
		{
			return new Language
			{
				Name = name,
				Culture = culture,
				AttributesXml = Language.Default.AttributesXml,
				StatementsXml = Language.Default.StatementsXml,
				QuestionsXml = Language.Default.QuestionsXml,
				ExtensionsXml = new List<LanguageExtension>(),
			};
		}
	}
}
