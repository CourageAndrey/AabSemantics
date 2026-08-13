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
	/// <summary>
	/// All strings the engine needs in one language: the core vocabulary for attributes,
	/// statements and questions, plus whatever the loaded modules contribute through
	/// <see cref="Extensions"/>. Languages are data, so new ones can be added as XML files
	/// without recompiling.
	/// </summary>
	public interface ILanguage
	{
		/// <summary>
		/// Display name of the language, written in that language itself.
		/// </summary>
		String Name
		{ get; }

		/// <summary>
		/// Culture identifier, e.g. <c>en-US</c>. Used to match the language against the
		/// current UI culture.
		/// </summary>
		String Culture
		{ get; }

		/// <summary>
		/// Names of the built-in concept attributes.
		/// </summary>
		ILanguageAttributes Attributes
		{ get; }

		/// <summary>
		/// Wordings used to describe statements and report consistency problems.
		/// </summary>
		ILanguageStatements Statements
		{ get; }

		/// <summary>
		/// Wordings used to phrase questions and their answers.
		/// </summary>
		ILanguageQuestions Questions
		{ get; }

		/// <summary>
		/// Per-module string bundles, one for each module that contributes its own vocabulary.
		/// </summary>
		ICollection<LanguageExtension> Extensions
		{ get; }
	}

	/// <summary>
	/// Loading languages from disk, choosing the one matching the current culture, and reaching
	/// into module-specific string bundles.
	/// </summary>
	public static class LanguagesExtensions
	{
		#region Constants

		/// <summary>
		/// File mask language definitions are searched by.
		/// </summary>
		public const String DefaultFileFormat = "*.xml";

		/// <summary>
		/// Folder, relative to the application directory, language definitions are loaded from.
		/// </summary>
		public const String DefaultFolderPath = "Localization";

		#endregion

		#region Extension routines

		/// <summary>
		/// Returns the language's string bundle for a given module.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The matching extension.</returns>
		/// <exception cref="InvalidOperationException">The module's extension is not registered with this language.</exception>
		public static ExtensionT GetExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageExtension
		{
			return language.Extensions.OfType<ExtensionT>().First();
		}

		/// <summary>
		/// Returns a module's attribute names, typed as the general interface.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's attribute names.</returns>
		public static ILanguageExtensionAttributes GetAttributesExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageAttributesExtension
		{
			return language.GetExtension<ExtensionT>().Attributes;
		}

		/// <summary>
		/// Returns a module's attribute names in the module's own type, sparing the caller a cast.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <typeparam name="AttributesT">Concrete attribute-names type the module declares.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's attribute names.</returns>
		public static AttributesT GetAttributesExtension<ExtensionT, AttributesT>(this ILanguage language)
			where ExtensionT : ILanguageAttributesExtension<AttributesT>
			where AttributesT : ILanguageExtensionAttributes
		{
			return language.GetExtension<ExtensionT>().Attributes;
		}

		/// <summary>
		/// Returns a module's concept names, typed as the general interface.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's concept names.</returns>
		public static ILanguageExtensionConcepts GetConceptsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageConceptsExtension
		{
			return language.GetExtension<ExtensionT>().Concepts;
		}

		/// <summary>
		/// Returns a module's concept names in the module's own type, sparing the caller a cast.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <typeparam name="ConceptsT">Concrete concept-names type the module declares.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's concept names.</returns>
		public static ConceptsT GetConceptsExtension<ExtensionT, ConceptsT>(this ILanguage language)
			where ExtensionT : ILanguageConceptsExtension<ConceptsT>
			where ConceptsT : ILanguageExtensionConcepts
		{
			return language.GetExtension<ExtensionT>().Concepts;
		}

		/// <summary>
		/// Returns a module's statement wordings, typed as the general interface.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's statement wordings.</returns>
		public static ILanguageExtensionStatements GetStatementsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageStatementsExtension
		{
			return language.GetExtension<ExtensionT>().Statements;
		}

		/// <summary>
		/// Returns a module's statement wordings in the module's own type, sparing the caller a cast.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <typeparam name="StatementsT">Concrete statement-wordings type the module declares.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's statement wordings.</returns>
		public static StatementsT GetStatementsExtension<ExtensionT, StatementsT>(this ILanguage language)
			where ExtensionT : ILanguageStatementsExtension<StatementsT>
			where StatementsT : ILanguageExtensionStatements
		{
			return language.GetExtension<ExtensionT>().Statements;
		}

		/// <summary>
		/// Returns a module's question wordings, typed as the general interface.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's question wordings.</returns>
		public static ILanguageExtensionQuestions GetQuestionsExtension<ExtensionT>(this ILanguage language)
			where ExtensionT : ILanguageQuestionsExtension
		{
			return language.GetExtension<ExtensionT>().Questions;
		}

		/// <summary>
		/// Returns a module's question wordings in the module's own type, sparing the caller a cast.
		/// </summary>
		/// <typeparam name="ExtensionT">Extension type contributed by the module.</typeparam>
		/// <typeparam name="QuestionsT">Concrete question-wordings type the module declares.</typeparam>
		/// <param name="language">Language to search.</param>
		/// <returns>The module's question wordings.</returns>
		public static QuestionsT GetQuestionsExtension<ExtensionT, QuestionsT>(this ILanguage language)
			where ExtensionT : ILanguageQuestionsExtension<QuestionsT>
			where QuestionsT : ILanguageExtensionQuestions
		{
			return language.GetExtension<ExtensionT>().Questions;
		}

		#endregion

		/// <summary>
		/// Loads user-supplied language definitions from disk. The folder is created when it
		/// does not exist, so a first run simply finds no additional languages.
		/// </summary>
		/// <param name="applicationPath">Application directory the folder is resolved against.</param>
		/// <param name="folderPath">Folder to scan, relative to the application directory.</param>
		/// <param name="fileFormat">File mask to match.</param>
		/// <returns>Languages deserialized from the matching files.</returns>
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

		/// <summary>
		/// Picks the language matching the current thread's UI culture.
		/// </summary>
		/// <param name="languages">Languages to choose from.</param>
		/// <param name="default">Language to fall back to when no culture matches.</param>
		/// <returns>The matching language, or <paramref name="default"/>.</returns>
		public static ILanguage FindAppropriate(this IEnumerable<ILanguage> languages, Language @default)
		{
			return languages.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? @default;
		}

		/// <summary>
		/// Resolves a string by a textual path, used to bind UI elements to localized text
		/// without compile-time references.
		/// </summary>
		/// <param name="language">Language to read from.</param>
		/// <param name="path">
		/// Either <c>Property.Nested</c> to address the language itself, or
		/// <c>Module\Property.Nested</c> to address a module's extension. The module part matches
		/// an extension named <c>Language{Module}Module</c>, falling back to its plain type name.
		/// </param>
		/// <returns>
		/// The resolved string, or <c>null</c> when any step of the path is missing or the
		/// final value is not a string.
		/// </returns>
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
				languageObject = language.Extensions.FirstOrDefault(e => e.GetType().Name == $"Language{moduleName}Module")
					?? language.Extensions.FirstOrDefault(e => e.GetType().Name == moduleName); // the WPF UI extension is named WpfUiModule

				propertyPath = pathParts[1].Split('.');
			}

			foreach (String member in propertyPath)
			{
				if (languageObject == null)
				{
					break;
				}

				var property = languageObject.GetType().GetProperty(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
				if (property == null)
				{
					return null;
				}

				languageObject = property.GetValue(languageObject);
			}

			return languageObject as String;
		}
	}
}
