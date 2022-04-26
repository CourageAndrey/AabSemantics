using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Utils;

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
				language.SerializeToFile(testFileName);
				restored = testFileName.DeserializeFromFile<Language>();
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
			var serializationError = Assert.Throws<InvalidOperationException>(() => { language.SerializeToFile(testFileName); });

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
				language.SerializeToFile(testFileName);
				restored = testFileName.DeserializeFromFile<Language>();
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
	}
}
