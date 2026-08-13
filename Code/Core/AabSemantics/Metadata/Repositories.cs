using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// The process-wide registries of everything modules contribute: attributes, statements,
	/// questions and answers.
	/// <para>
	/// This state is static and therefore shared by every semantic network in the process, which
	/// is why <see cref="IExtensionModule.RegisterMetadata"/> runs only once per module. Tests
	/// that need a clean slate call <see cref="Reset"/>.
	/// </para>
	/// </summary>
	public static class Repositories
	{
		/// <summary>
		/// Modules whose metadata has been registered, keyed by module name.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IDictionary<String, IExtensionModule> Modules
		{
			get { return _modules; }
			set { _modules = value.EnsureNotNull(nameof(value)); }
		}

		/// <summary>
		/// Registered attribute definitions.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IMetadataRepository<AttributeDefinition> Attributes
		{
			get { return _attributes; }
			set { _attributes = value.EnsureNotNull(nameof(value)); }
		}

		/// <summary>
		/// Registered statement definitions, keyed by statement type.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IMetadataRepository<StatementDefinition> Statements
		{
			get { return _statements; }
			set { _statements = value.EnsureNotNull(nameof(value)); }
		}

		/// <summary>
		/// Registered custom statement definitions, keyed by kind. These are separate from
		/// <see cref="Statements"/> because they all share one statement type.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IDictionary<String, CustomStatementDefinition> CustomStatements
		{
			get { return _customStatements; }
			set { _customStatements = value.EnsureNotNull(nameof(value)); }
		}

		/// <summary>
		/// Registered question definitions, keyed by question type.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IMetadataRepository<QuestionDefinition> Questions
		{
			get { return _questions; }
			set { _questions = value.EnsureNotNull(nameof(value)); }
		}

		/// <summary>
		/// Registered answer definitions, keyed by answer type.
		/// </summary>
		/// <exception cref="ArgumentNullException">The assigned value is <c>null</c>.</exception>
		public static IMetadataRepository<AnswerDefinition> Answers
		{
			get { return _answers; }
			set { _answers = value.EnsureNotNull(nameof(value)); }
		}

		private static IDictionary<String, IExtensionModule> _modules;
		private static IMetadataRepository<AttributeDefinition> _attributes;
		private static IMetadataRepository<StatementDefinition> _statements;
		private static IDictionary<String, CustomStatementDefinition> _customStatements;
		private static IMetadataRepository<QuestionDefinition> _questions;
		private static IMetadataRepository<AnswerDefinition> _answers;

		/// <summary>
		/// Registers an attribute kind, replacing any definition previously registered for the same type.
		/// </summary>
		/// <param name="type">Attribute type.</param>
		/// <param name="value">The shared instance of that type.</param>
		/// <param name="nameGetter">Selects the attribute's display name from a language.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static AttributeDefinition RegisterAttribute(
			Type type,
			IAttribute value,
			Func<ILanguage, String> nameGetter)
		{
			var definition = new AttributeDefinition(type, value, nameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers an attribute kind, inferring its type from the type argument.
		/// </summary>
		/// <typeparam name="AttributeT">Attribute type.</typeparam>
		/// <param name="value">The shared instance of that type.</param>
		/// <param name="nameGetter">Selects the attribute's display name from a language.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static AttributeDefinition<AttributeT> RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> nameGetter)
			where AttributeT : IAttribute
		{
			var definition = new AttributeDefinition<AttributeT>(value, nameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers a statement type, replacing any definition previously registered for it.
		/// </summary>
		/// <param name="type">Statement type.</param>
		/// <param name="nameGetter">Selects the statement's display name from a language.</param>
		/// <param name="formatTrue">Selects the affirmative wording from a language.</param>
		/// <param name="formatFalse">Selects the negative wording from a language.</param>
		/// <param name="formatQuestion">Selects the interrogative wording from a language.</param>
		/// <param name="getDescriptionParameters">Maps a statement to the knowledge items its wordings refer to by anchor.</param>
		/// <param name="consistencyChecker">
		/// Validates all statements of this type within a network; pass
		/// <see cref="StatementDefinition.NoConsistencyCheck"/> when there is nothing to check.
		/// </param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static StatementDefinition RegisterStatement(
			Type type,
			Func<ILanguage, String> nameGetter,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion,
			Func<IStatement, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			var definition = new StatementDefinition(
				type,
				nameGetter,
				formatTrue,
				formatFalse,
				formatQuestion,
				getDescriptionParameters,
				consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers a statement type whose wordings come from a module's language extension,
		/// so that the three format strings are derived instead of being passed one by one.
		/// </summary>
		/// <typeparam name="StatementT">Statement type.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <param name="partGetter">Picks this statement's wording out of a format-string group.</param>
		/// <param name="getDescriptionParameters">Maps a statement to the knowledge items its wordings refer to by anchor.</param>
		/// <param name="consistencyChecker">Validates all statements of this type within a network.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> RegisterStatement<StatementT, ModuleT, LanguageStatementsT, PartT>(
			Func<PartT, String> partGetter,
			Func<StatementT, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			var definition = new StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT>(
				partGetter,
				getDescriptionParameters,
				consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers a question type, replacing any definition previously registered for it.
		/// </summary>
		/// <param name="type">Question type.</param>
		/// <param name="nameGetter">Selects the question's display name from a language.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static QuestionDefinition RegisterQuestion(
			Type type,
			Func<ILanguage, String> nameGetter)
		{
			var definition = new QuestionDefinition(type, nameGetter);
			Questions.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers a question type, inferring it from the type argument.
		/// </summary>
		/// <typeparam name="QuestionT">Question type.</typeparam>
		/// <param name="nameGetter">Selects the question's display name from a language.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static QuestionDefinition<QuestionT> RegisterQuestion<QuestionT>(
			Func<ILanguage, String> nameGetter)
			where QuestionT : IQuestion
		{
			var definition = new QuestionDefinition<QuestionT>(nameGetter);
			Questions.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers an answer type, replacing any definition previously registered for it.
		/// </summary>
		/// <param name="type">Answer type.</param>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static AnswerDefinition RegisterAnswer(Type type)
		{
			var definition = new AnswerDefinition(type);
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers a custom statement kind, taking its identifier from the type argument's name
		/// and its wordings from a module's language extension.
		/// </summary>
		/// <typeparam name="StatementT">Type whose name becomes the statement kind identifier.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <param name="concepts">Names of the roles the statement relates.</param>
		/// <param name="partGetter">Picks this statement's wording out of a format-string group.</param>
		/// <returns>The created definition.</returns>
		public static CustomStatementDefinition RegisterCustomStatement<StatementT, ModuleT, LanguageStatementsT, PartT>(
			ICollection<String> concepts,
			Func<PartT, String> partGetter)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			return RegisterCustomStatement(
				typeof(StatementT).Name,
				concepts,
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().TrueFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().FalseFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().QuestionFormatStrings));
		}

		/// <summary>
		/// Registers a custom statement kind, replacing any kind previously registered under
		/// the same identifier.
		/// </summary>
		/// <param name="type">Identifier of the statement kind.</param>
		/// <param name="concepts">Names of the roles the statement relates.</param>
		/// <param name="formatTrue">Selects the affirmative wording from a language.</param>
		/// <param name="formatFalse">Selects the negative wording from a language.</param>
		/// <param name="formatQuestion">Selects the interrogative wording from a language.</param>
		/// <returns>The created definition.</returns>
		public static CustomStatementDefinition RegisterCustomStatement(
			String type,
			ICollection<String> concepts,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion)
		{
			var definition = new CustomStatementDefinition(
				type,
				concepts,
				formatTrue,
				formatFalse,
				formatQuestion);
			_customStatements[type] = definition;
			return definition;
		}

		/// <summary>
		/// Registers an answer type, inferring it from the type argument.
		/// </summary>
		/// <typeparam name="AnswerT">Answer type.</typeparam>
		/// <returns>The created definition, for further fluent configuration.</returns>
		public static AnswerDefinition<AnswerT> RegisterAnswer<AnswerT>()
			where AnswerT : IAnswer
		{
			var definition = new AnswerDefinition<AnswerT>();
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}

		/// <summary>
		/// Discards every registration and restores the built-in ones. Because the registries are
		/// static, this also invalidates any semantic network relying on modules registered so far;
		/// it exists mainly to isolate tests from one another.
		/// </summary>
		public static void Reset()
		{
			_modules = new Dictionary<String, IExtensionModule>();
			_attributes = new MetadataRepository<AttributeDefinition>();
			_statements = new MetadataRepository<StatementDefinition>();
			_customStatements = new Dictionary<String, CustomStatementDefinition>();
			_questions = new MetadataRepository<QuestionDefinition>();
			_answers = new MetadataRepository<AnswerDefinition>();
			InitializeCustomStatement();
			InitializeCustomStatementQuestion();
		}

		static Repositories()
		{
			Reset();
		}

		/// <summary>
		/// Registers the built-in custom statement type together with its XML and JSON persistence.
		/// Its three wording getters throw, because a custom statement's wordings come from its
		/// own <see cref="CustomStatementDefinition"/> rather than from this shared type.
		/// </summary>
		public static void InitializeCustomStatement()
		{
			RegisterStatement(
					typeof(Statements.CustomStatement),
					language => language.Statements.CustomStatementName,
					language => throw new NotSupportedException(),
					language => throw new NotSupportedException(),
					language => throw new NotSupportedException(),
					statement => ((Statements.CustomStatement) statement).Concepts.ToDictionary(
						p => $"#{p.Key}#",
						p => p.Value as IKnowledge),
					StatementDefinition.NoConsistencyCheck)
				.SerializeToXml(statement => new Serialization.Xml.CustomStatement((Statements.CustomStatement) statement), typeof(Serialization.Xml.CustomStatement))
				.SerializeToJson(statement => new Serialization.Json.CustomStatement((Statements.CustomStatement) statement), typeof(Serialization.Json.CustomStatement));
		}

		/// <summary>
		/// Registers the built-in question that checks whether a custom statement holds,
		/// together with its XML and JSON persistence.
		/// </summary>
		public static void InitializeCustomStatementQuestion()
		{
			RegisterQuestion<Questions.CustomStatementQuestion>(language => language.Questions.CustomStatementQuestionName)
				.SerializeToXml(question => new Serialization.Xml.CustomStatementQuestion(question))
				.SerializeToJson(question => new Serialization.Json.CustomStatementQuestion(question));
		}
	}
}
