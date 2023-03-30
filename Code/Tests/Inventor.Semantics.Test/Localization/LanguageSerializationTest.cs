using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Serialization.Xml;
using Inventor.Semantics.Set.Localization;

namespace Inventor.Semantics.Test.Localization
{
	[TestFixture]
	public class LanguageSerializationTest
	{
		[TearDown]
		public void RestoreDefaultLanguage()
		{
			Language.Default.Extensions.Clear();
			Repositories.Modules.Clear();
		}

		[Test]
		public void SerializeDefaultLanguage()
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
			assertLanuagesAreEqual(language, restored);
		}

		[Test]
		public void ImpossibleToSerializeLanguageWithUnregisteredExtension()
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
		public void SerializeLanguageWithRegisteredExtension()
		{
			// arrange
			var language = Language.Default;
			string testFileName = Path.GetTempFileName();

			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
			new Set.SetModule().RegisterMetadata();

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
			assertLanuagesAreEqual(language, restored);
		}

		private static void assertLanuagesAreEqual(Language language, Language restored)
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
		public void CheckAdditionalLanguages()
		{
			// arrange
			string tempFolder = Path.ChangeExtension(Path.GetTempFileName(), string.Empty);
			var additionalLanguages = new[]
			{
				createTestLanguage("Deutsch", "de"),
				createTestLanguage("Français", "fr"),
				createTestLanguage("Español", "es"),
				createTestLanguage("Ελληνικά", "el"),
				createTestLanguage("فارسی", "fa"),
				createTestLanguage("ქართული", "ka"),
				createTestLanguage("中文", "zh"),
				createTestLanguage("日本語", "ja"),
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
		public void CheckAdditionalLanguagesFolderCreation()
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

		private static Language createTestLanguage(string name, string culture)
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
