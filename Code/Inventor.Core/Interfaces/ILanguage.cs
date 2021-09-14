using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inventor.Core.Localization;
using Inventor.Core.Utils;

namespace Inventor.Core
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
			return languagesFolder.GetFiles(fileFormat).Select(f => f.FullName.DeserializeFromFile<Language>() as ILanguage).ToList();
		}

		public static ILanguage FindAppropriate(this IEnumerable<ILanguage> languages, Language @default)
		{
			return languages.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? @default;
		}
	}
}
