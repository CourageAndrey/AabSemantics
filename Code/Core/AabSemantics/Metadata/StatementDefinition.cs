using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Text.Primitives;
using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// Validates a whole semantic network with respect to one statement type, appending a
	/// description of each problem found to <paramref name="result"/>.
	/// </summary>
	/// <param name="semanticNetwork">Network to validate.</param>
	/// <param name="result">Container problem descriptions are appended to.</param>
	/// <param name="cancellationToken">Cancels the check, which may walk the whole network.</param>
	public delegate Task StatementConsistencyCheckerDelegate(ISemanticNetwork semanticNetwork, ITextContainer result, CancellationToken cancellationToken);

	/// <summary>
	/// Strongly typed counterpart of <see cref="StatementConsistencyCheckerDelegate"/>: receives
	/// the network's statements of the relevant type already filtered.
	/// </summary>
	/// <typeparam name="StatementT">Statement type being validated.</typeparam>
	/// <param name="semanticNetwork">Network to validate.</param>
	/// <param name="result">Container problem descriptions are appended to.</param>
	/// <param name="statements">The network's statements of type <typeparamref name="StatementT"/>.</param>
	/// <param name="cancellationToken">Cancels the check, which may walk the whole network.</param>
	public delegate Task StatementConsistencyCheckerDelegate<StatementT>(ISemanticNetwork semanticNetwork, ITextContainer result, ICollection<StatementT> statements, CancellationToken cancellationToken)
		where StatementT : IStatement;

	/// <summary>
	/// JSON persistence settings for a statement type.
	/// </summary>
	public class StatementJsonSerializationSettings : IStatementSerializationSettings, IJsonSerializationSettings
	{
		/// <summary>
		/// JSON surrogate type statements are converted into.
		/// </summary>
		public Type JsonType
		{ get; }

		private readonly Func<IStatement, Serialization.Json.Statement> _serializer;

		/// <summary>
		/// Configures JSON persistence.
		/// </summary>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="jsonType">Surrogate type; must derive from the JSON statement base type.</param>
		public StatementJsonSerializationSettings(Func<IStatement, Serialization.Json.Statement> serializer, Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Statement>(nameof(jsonType));
		}

		/// <summary>
		/// Converts a statement into its JSON surrogate.
		/// </summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Json.Statement GetJson(IStatement statement)
		{
			return _serializer(statement);
		}
	}

	/// <summary>
	/// XML persistence settings for a statement type.
	/// </summary>
	public class StatementXmlSerializationSettings : IStatementSerializationSettings, IXmlSerializationSettings
	{
		/// <summary>
		/// Element name written to XML: the surrogate's type name without its "Statement" suffix.
		/// </summary>
		public String XmlElementName
		{ get; }

		/// <summary>
		/// XML surrogate type statements are converted into.
		/// </summary>
		public Type XmlType
		{ get; }

		private readonly Func<IStatement, Serialization.Xml.Statement> _serializer;

		/// <summary>
		/// Configures XML persistence.
		/// </summary>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="xmlType">Surrogate type; must derive from the XML statement base type.</param>
		public StatementXmlSerializationSettings(Func<IStatement, Serialization.Xml.Statement> serializer, Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Statement>(nameof(xmlType));
			XmlElementName = XmlType.Name.Replace("Statement", "");
		}

		/// <summary>
		/// Converts a statement into its XML surrogate.
		/// </summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Xml.Statement GetXml(IStatement statement)
		{
			return _serializer(statement);
		}
	}

	/// <summary>
	/// Runtime description of a statement type: its name, the three wordings used to render it,
	/// how to persist it, and how to validate a network containing it.
	/// </summary>
	public class StatementDefinition : MetadataDefinition<IStatementSerializationSettings>
	{
		#region Properties

		private readonly Func<ILanguage, String> _nameGetter;
		private readonly Func<ILanguage, String> _formatTrue;
		private readonly Func<ILanguage, String> _formatFalse;
		private readonly Func<ILanguage, String> _formatQuestion;
		private readonly Func<IStatement, IDictionary<String, IKnowledge>> _getDescriptionParameters;
		private readonly StatementConsistencyCheckerDelegate _consistencyChecker;

		#endregion

		#region Constructors

		/// <summary>
		/// Describes a statement type.
		/// </summary>
		/// <param name="type">Statement type being described; must implement <see cref="IStatement"/>.</param>
		/// <param name="nameGetter">Selects the statement's display name from a language.</param>
		/// <param name="formatTrue">Selects the affirmative wording from a language.</param>
		/// <param name="formatFalse">Selects the negative wording from a language.</param>
		/// <param name="formatQuestion">Selects the interrogative wording from a language.</param>
		/// <param name="getDescriptionParameters">Maps a statement to the knowledge items its wordings refer to by anchor.</param>
		/// <param name="consistencyChecker">Validates a network with respect to this statement type.</param>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
		public StatementDefinition(
			Type type,
			Func<ILanguage, String> nameGetter,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion,
			Func<IStatement, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate consistencyChecker)
			: base(type, typeof(IStatement))
		{
			_nameGetter = nameGetter.EnsureNotNull(nameof(nameGetter));
			_formatTrue = formatTrue.EnsureNotNull(nameof(formatTrue));
			_formatFalse = formatFalse.EnsureNotNull(nameof(formatFalse));
			_formatQuestion = formatQuestion.EnsureNotNull(nameof(formatQuestion));
			_getDescriptionParameters = getDescriptionParameters.EnsureNotNull(nameof(getDescriptionParameters));
			_consistencyChecker = consistencyChecker.EnsureNotNull(nameof(consistencyChecker));
		}

		#endregion

		/// <summary>
		/// Returns the statement type's display name in a language.
		/// </summary>
		/// <param name="language">Language to read the name in.</param>
		/// <returns>The localized name.</returns>
		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}

		/// <summary>
		/// Describes a statement as an affirmative sentence, with an anchor to the statement
		/// itself appended so the reader can navigate to it.
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public IText DescribeTrue(IStatement statement)
		{
			var formatter = new Func<ILanguage, String>(language => _formatTrue(language) + $" ({Strings.ParamStatement})");

			var parameters = _getDescriptionParameters(statement);
			parameters[Strings.ParamStatement] = statement;

			return new FormattedText(formatter, parameters);
		}

		/// <summary>
		/// Describes a statement as a negative sentence.
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public IText DescribeFalse(IStatement statement)
		{
			return new FormattedText(language => _formatFalse(language), _getDescriptionParameters(statement));
		}

		/// <summary>
		/// Describes a statement as a question.
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public IText DescribeQuestion(IStatement statement)
		{
			return new FormattedText(language => _formatQuestion(language), _getDescriptionParameters(statement));
		}

		/// <summary>
		/// Validates a network with respect to this statement type.
		/// </summary>
		/// <param name="semanticNetwork">Network to validate.</param>
		/// <param name="result">Container problem descriptions are appended to.</param>
		/// <param name="cancellationToken">Cancels the check.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task CheckConsistencyAsync(ISemanticNetwork semanticNetwork, ITextContainer result, CancellationToken cancellationToken = default)
		{
			await _consistencyChecker(semanticNetwork, result, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Blocking counterpart of <see cref="CheckConsistencyAsync"/>, for callers that cannot await.
		/// </summary>
		/// <param name="semanticNetwork">Network to validate.</param>
		/// <param name="result">Container problem descriptions are appended to.</param>
		/// <param name="cancellationToken">Cancels the check.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public void CheckConsistency(ISemanticNetwork semanticNetwork, ITextContainer result, CancellationToken cancellationToken = default)
		{
			TaskHelper.AwaitDetached(() => CheckConsistencyAsync(semanticNetwork, result, cancellationToken));
		}

		/// <summary>
		/// A checker that reports nothing, for statement types with no consistency rules.
		/// The constructor rejects a <c>null</c> checker, so pass this instead.
		/// </summary>
		public static readonly StatementConsistencyCheckerDelegate NoConsistencyCheck = (semanticNetwork, result, cancellationToken) => Task.CompletedTask;
	}

	/// <summary>
	/// A statement definition that derives all four wordings from a module's language extension,
	/// and narrows the consistency check to statements of the described type.
	/// </summary>
	/// <typeparam name="StatementT">Statement type being described.</typeparam>
	/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
	/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
	/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
	public class StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> : StatementDefinition
		where StatementT : class, IStatement
		where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
		where LanguageStatementsT : ILanguageExtensionStatements<PartT>
	{
		/// <summary>
		/// Describes a statement type whose wordings live in a module's language extension.
		/// </summary>
		/// <param name="partGetter">Picks this statement's wording out of a format-string group.</param>
		/// <param name="getDescriptionParameters">Maps a statement to the knowledge items its wordings refer to by anchor.</param>
		/// <param name="consistencyChecker">Validates the network's statements of this type.</param>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
		public StatementDefinition(
			Func<PartT, String> partGetter,
			Func<StatementT, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			: base(
				typeof(StatementT),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().Names),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().TrueFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().FalseFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().QuestionFormatStrings),
				statement => getDescriptionParameters(statement as StatementT),
				(semanticNetwork, result, cancellationToken) => consistencyChecker(semanticNetwork, result, semanticNetwork.Statements.OfType<StatementT>().ToList(), cancellationToken))
		{
			partGetter.EnsureNotNull(nameof(partGetter));
			getDescriptionParameters.EnsureNotNull(nameof(getDescriptionParameters));
			consistencyChecker.EnsureNotNull(nameof(consistencyChecker));
		}

		/// <summary>
		/// A typed checker that reports nothing, for statement types with no consistency rules.
		/// Hides the untyped <see cref="StatementDefinition.NoConsistencyCheck"/>.
		/// </summary>
		public new static readonly StatementConsistencyCheckerDelegate<StatementT> NoConsistencyCheck = (semanticNetwork, result, statements, cancellationToken) => Task.CompletedTask;
	}

	/// <summary>
	/// Fluent configuration of how a statement type is persisted.
	/// </summary>
	public static class StatementDefinitionExtensions
	{
		/// <summary>
		/// Returns the definition's XML settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The XML settings.</returns>
		/// <exception cref="InvalidOperationException">XML serialization has not been configured.</exception>
		public static IXmlSerializationSettings GetXmlSerializationSettings(this StatementDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<StatementXmlSerializationSettings>();
		}

		/// <summary>
		/// Returns the definition's JSON settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The JSON settings.</returns>
		/// <exception cref="InvalidOperationException">JSON serialization has not been configured.</exception>
		public static IJsonSerializationSettings GetJsonSerializationSettings(this StatementDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<StatementJsonSerializationSettings>();
		}

		/// <summary>
		/// Configures XML persistence for an untyped definition.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="xmlType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static StatementDefinition SerializeToXml(
			this StatementDefinition metadataDefinition,
			Func<IStatement, Serialization.Xml.Statement> serializer,
			Type xmlType)
		{
			metadataDefinition.SerializationSettings.Add(new StatementXmlSerializationSettings(
				serializer,
				xmlType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence for an untyped definition.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="jsonType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static StatementDefinition SerializeToJson(
			this StatementDefinition metadataDefinition,
			Func<IStatement, Serialization.Json.Statement> serializer,
			Type jsonType)
		{
			metadataDefinition.SerializationSettings.Add(new StatementJsonSerializationSettings(
				serializer,
				jsonType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures XML persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="StatementT">Statement type being configured.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="xmlType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <c>null</c>.</exception>
		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> SerializeToXml<StatementT, ModuleT, LanguageStatementsT, PartT>(
			this StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> metadataDefinition,
			Func<StatementT, Serialization.Xml.Statement> serializer,
			Type xmlType)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			serializer.EnsureNotNull(nameof(serializer));
			metadataDefinition.SerializationSettings.Add(new StatementXmlSerializationSettings(
				statement => serializer((StatementT) statement),
				xmlType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="StatementT">Statement type being configured.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <param name="jsonType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <c>null</c>.</exception>
		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> SerializeToJson<StatementT, ModuleT, LanguageStatementsT, PartT>(
			this StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> metadataDefinition,
			Func<StatementT, Serialization.Json.Statement> serializer,
			Type jsonType)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			serializer.EnsureNotNull(nameof(serializer));
			metadataDefinition.SerializationSettings.Add(new StatementJsonSerializationSettings(
				statement => serializer((StatementT) statement),
				jsonType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures XML persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="StatementT">Statement type being configured.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <typeparam name="XmlT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> SerializeToXml<StatementT, ModuleT, LanguageStatementsT, PartT, XmlT>(
			this StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> metadataDefinition,
			Func<StatementT, XmlT> serializer)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
			where XmlT : Serialization.Xml.Statement
		{
			return metadataDefinition.SerializeToXml(
				serializer,
				typeof(XmlT));
		}

		/// <summary>
		/// Configures JSON persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="StatementT">Statement type being configured.</typeparam>
		/// <typeparam name="ModuleT">Language extension contributed by the owning module.</typeparam>
		/// <typeparam name="LanguageStatementsT">The extension's statement-wordings type.</typeparam>
		/// <typeparam name="PartT">Group of format strings the wording is selected from.</typeparam>
		/// <typeparam name="JsonT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a statement into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> SerializeToJson<StatementT, ModuleT, LanguageStatementsT, PartT, JsonT>(
			this StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> metadataDefinition,
			Func<StatementT, JsonT> serializer)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
			where JsonT : Serialization.Json.Statement
		{
			return metadataDefinition.SerializeToJson(
				serializer,
				typeof(JsonT));
		}
	}
}
