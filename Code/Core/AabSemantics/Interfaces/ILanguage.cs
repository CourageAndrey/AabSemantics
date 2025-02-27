using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using AabSemantics.Localization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics
{
	public interface ILanguage
	{
		String Name
		{ get; }

		String Culture
		{ get; }

		ILanguageAttributes Attributes
		{ get; }

		ILanguageStatements Statements
		{ get; }

		ILanguageQuestions Questions
		{ get; }

		ICollection<LanguageExtension> Extensions
		{ get; }
	}

	public static class LanguagesExtensions
	{
		#region Constants

		public const String DefaultFileFormat = "*.xml";
		public const String DefaultFolderPath = "Localization";

		#endregion

		public static ExtensionT GetExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageExtension
		{
			return language.Extensions.OfType<ExtensionT>().First();
		}

		public static ICollection<ILanguage> LoadAdditionalLanguages(
			this String applicationPath,
			String folderPath = DefaultFolderPath,
			String fileFormat = DefaultFileFormat)
		{
			var languagesFolder = new DirectoryInfo(Path.Combine(applicationPath, folderPath));
			if (!languagesFolder.Exists)
			{
				languagesFolder.Create();
			}
			return languagesFolder.GetFiles(fileFormat).Select(f => f.FullName.DeserializeFromXmlFile<Language>() as ILanguage).ToList();
		}

		public static ILanguage FindAppropriate(this IEnumerable<ILanguage> languages, Language @default)
		{
			return languages.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? @default;
		}

		public static String GetBoundText(this ILanguage language, String path)
		{
			Object languageObject = null;
			String[] propertyPath = Array.Empty<String>();

			var pathParts = path.Split('\\');
			if (pathParts.Length == 1)
			{
				languageObject = language;

				propertyPath = pathParts[0].Split('.');
			}
			else // if (pathParts.Length >= 2) because this expression is always true (zero is impossible)
			{
				String moduleName = pathParts[0];
				languageObject = language.Extensions.FirstOrDefault(e => e.GetType().Name == $"Language{moduleName}Module");

				propertyPath = pathParts[1].Split('.');
			}

			foreach (String member in propertyPath)
			{
				if (languageObject == null)
				{
					break;
				}

				var property = languageObject.GetType().GetProperty(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

				languageObject = property.GetValue(languageObject);
			}

			return languageObject as String;
		}
	}
}
