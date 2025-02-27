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

		#region Extension routines

		public static ExtensionT GetExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageExtension
		{
			return language.Extensions.OfType<ExtensionT>().First();
		}

		public static ILanguageExtensionAttributes GetAttributesExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageAttributesExtension
		{
			return language.GetExtension<ExtensionT>().Attributes;
		}

		public static AttributesT GetAttributesExtension<ExtensionT, AttributesT>(this ILanguage language)
			where ExtensionT : ILanguageAttributesExtension<AttributesT>
			where AttributesT : ILanguageExtensionAttributes
		{
			return language.GetExtension<ExtensionT>().Attributes;
		}

		public static ILanguageExtensionConcepts GetConceptsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageConceptsExtension
		{
			return language.GetExtension<ExtensionT>().Concepts;
		}

		public static ConceptsT GetConceptsExtension<ExtensionT, ConceptsT>(this ILanguage language)
			where ExtensionT : ILanguageConceptsExtension<ConceptsT>
			where ConceptsT : ILanguageExtensionConcepts
		{
			return language.GetExtension<ExtensionT>().Concepts;
		}

		public static ILanguageExtensionStatements GetStatementsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageStatementsExtension
		{
			return language.GetExtension<ExtensionT>().Statements;
		}

		public static StatementsT GetStatementsExtension<ExtensionT, StatementsT>(this ILanguage language)
			where ExtensionT : ILanguageStatementsExtension<StatementsT>
			where StatementsT : ILanguageExtensionStatements
		{
			return language.GetExtension<ExtensionT>().Statements;
		}

		public static ILanguageExtensionQuestions GetQuestionsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageQuestionsExtension
		{
			return language.GetExtension<ExtensionT>().Questions;
		}

		public static QuestionsT GetQuestionsExtension<ExtensionT, QuestionsT>(this ILanguage language)
			where ExtensionT : ILanguageQuestionsExtension<QuestionsT>
			where QuestionsT : ILanguageExtensionQuestions
		{
			return language.GetExtension<ExtensionT>().Questions;
		}

		#endregion

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
