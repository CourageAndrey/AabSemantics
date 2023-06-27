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
using AabSemantics.Modules.Set.Localization;
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
			Assert.IsNull(restored.FileName);
			AssertLanguagesAreEqual(language, restored);
		}

		[Test]
		public void GivenUnregisteredExtension_WhenTryToSerializeLanguage_ThenFail()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			language.Extensions.Add(LanguageSetModule.CreateDefault());

			// act
			var serializationError = Assert.Throws<InvalidOperationException>(() => { language.SerializeToXmlFile(testFileName); });

			// assert
			Assert.IsTrue(serializationError.Message.Contains("XML"));

			string serializationErrorDetails = serializationError.InnerException.Message;
			Assert.IsTrue(serializationErrorDetails.Contains(typeof(XmlIncludeAttribute).Name.Replace("Attribute", string.Empty)));
		}

		[Test]
		public void GivenRegisteredExtension_WhenSerializeLanguage_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
			new Modules.Set.SetModule().RegisterMetadata();

			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			language.Extensions.Add(LanguageSetModule.CreateDefault());

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
			Assert.IsNull(restored.FileName);
			AssertLanguagesAreEqual(language, restored);
		}

		private static void AssertLanguagesAreEqual(Language language, Language restored)
		{
			Assert.IsNull(restored.FileName);
			Assert.AreEqual(language.Culture, restored.Culture);
			Assert.AreEqual(language.Name, restored.Name);

			language.Attributes.AssertPropertiesAreEqual(restored.Attributes);
			language.Statements.AssertPropertiesAreEqual(restored.Statements);
			language.Questions.AssertPropertiesAreEqual(restored.Questions);

			Assert.AreEqual(language.Extensions.Count, restored.Extensions.Count);
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
				Assert.IsTrue(languagesDictionary.SequenceEqual(deserializedDictionary));
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
				Assert.IsTrue(Directory.Exists(Path.Combine(tempFolder, LanguagesExtensions.DefaultFolderPath)));
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
