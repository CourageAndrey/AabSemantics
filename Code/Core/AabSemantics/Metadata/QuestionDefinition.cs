using System;

using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// JSON persistence settings for a question type: the surrogate type and the function
	/// converting a question into it. Questions carry no rendered text, so no language is needed.
	/// </summary>
	public class QuestionJsonSerializationSettings : IQuestionSerializationSettings, IJsonSerializationSettings
	{
		/// <summary>
		/// JSON surrogate type questions are converted into.
		/// </summary>
		public Type JsonType
		{ get; }

		private readonly Func<IQuestion, Serialization.Json.Question> _serializer;

		/// <summary>
		/// Configures JSON persistence.
		/// </summary>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <param name="jsonType">Surrogate type; must derive from the JSON question base type.</param>
		public QuestionJsonSerializationSettings(
			Func<IQuestion, Serialization.Json.Question> serializer,
			Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Question>(nameof(jsonType));
		}

		/// <summary>
		/// Converts a question into its JSON surrogate.
		/// </summary>
		/// <param name="question">Question to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Json.Question GetJson(IQuestion question)
		{
			return _serializer(question);
		}
	}

	/// <summary>
	/// XML persistence settings for a question type: the surrogate type and the function
	/// converting a question into it.
	/// </summary>
	public class QuestionXmlSerializationSettings : IQuestionSerializationSettings, IXmlSerializationSettings
	{
		/// <summary>
		/// XML surrogate type questions are converted into.
		/// </summary>
		public Type XmlType
		{ get; }

		private readonly Func<IQuestion, Serialization.Xml.Question> _serializer;

		/// <summary>
		/// Configures XML persistence.
		/// </summary>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <param name="xmlType">Surrogate type; must derive from the XML question base type.</param>
		public QuestionXmlSerializationSettings(
			Func<IQuestion, Serialization.Xml.Question> serializer,
			Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Question>(nameof(xmlType));
		}

		/// <summary>
		/// Converts a question into its XML surrogate.
		/// </summary>
		/// <param name="question">Question to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Xml.Question GetXml(IQuestion question)
		{
			return _serializer(question);
		}
	}

	/// <summary>
	/// Runtime description of a question type: how to name it in each language and how to
	/// persist it. Registering one is what makes a question selectable in a user interface.
	/// </summary>
	public class QuestionDefinition : MetadataDefinition<IQuestionSerializationSettings>
	{
		#region Properties

		private readonly Func<ILanguage, String> _nameGetter;

		#endregion

		/// <summary>
		/// Describes a question type.
		/// </summary>
		/// <param name="type">Question type being described; must implement <see cref="IQuestion"/>.</param>
		/// <param name="nameGetter">Selects the question's display name from a language.</param>
		public QuestionDefinition(Type type, Func<ILanguage, String> nameGetter)
			: base(type, typeof(IQuestion))
		{
			_nameGetter = nameGetter.EnsureNotNull(nameof(nameGetter));
		}

		/// <summary>
		/// Returns the question's display name in a language.
		/// </summary>
		/// <param name="language">Language to read the name in.</param>
		/// <returns>The localized name.</returns>
		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}
	}

	/// <summary>
	/// Strongly typed <see cref="QuestionDefinition"/>, sparing the caller an explicit type argument.
	/// </summary>
	/// <typeparam name="QuestionT">Question type being described.</typeparam>
	public class QuestionDefinition<QuestionT> : QuestionDefinition
		where QuestionT : IQuestion
	{
		/// <summary>
		/// Describes the question type.
		/// </summary>
		/// <param name="nameGetter">Selects the question's display name from a language.</param>
		public QuestionDefinition(Func<ILanguage, String> nameGetter)
			: base(typeof(QuestionT), nameGetter)
		{ }
	}

	/// <summary>
	/// Fluent configuration of how a question type is persisted.
	/// </summary>
	public static class QuestionDefinitionExtensions
	{
		/// <summary>
		/// Returns the definition's XML settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The XML settings.</returns>
		/// <exception cref="InvalidOperationException">XML serialization has not been configured.</exception>
		public static IXmlSerializationSettings GetXmlSerializationSettings(this QuestionDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<QuestionXmlSerializationSettings>();
		}

		/// <summary>
		/// Returns the definition's JSON settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The JSON settings.</returns>
		/// <exception cref="InvalidOperationException">JSON serialization has not been configured.</exception>
		public static IJsonSerializationSettings GetJsonSerializationSettings(this QuestionDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<QuestionJsonSerializationSettings>();
		}

		/// <summary>
		/// Configures XML persistence for an untyped definition.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <param name="xmlType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition SerializeToXml(
			this QuestionDefinition metadataDefinition,
			Func<IQuestion, Serialization.Xml.Question> serializer,
			Type xmlType)
		{
			metadataDefinition.SerializationSettings.Add(new QuestionXmlSerializationSettings(serializer, xmlType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence for an untyped definition.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="questionJsonGetter">Converts a question into its surrogate.</param>
		/// <param name="jsonType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition SerializeToJson(
			this QuestionDefinition metadataDefinition,
			Func<IQuestion, Serialization.Json.Question> questionJsonGetter,
			Type jsonType)
		{
			metadataDefinition.SerializationSettings.Add(new QuestionJsonSerializationSettings(questionJsonGetter, jsonType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures XML persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="QuestionT">Question type being configured.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <param name="xmlType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition<QuestionT> SerializeToXml<QuestionT>(
			this QuestionDefinition<QuestionT> metadataDefinition,
			Func<QuestionT, Serialization.Xml.Question> serializer,
			Type xmlType)
			where QuestionT : IQuestion
		{
			metadataDefinition.SerializationSettings.Add(new QuestionXmlSerializationSettings(
				question => serializer((QuestionT) question),
				xmlType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="QuestionT">Question type being configured.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <param name="jsonType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition<QuestionT> SerializeToJson<QuestionT>(
			this QuestionDefinition<QuestionT> metadataDefinition,
			Func<QuestionT, Serialization.Json.Question> serializer,
			Type jsonType)
			where QuestionT : IQuestion
		{
			metadataDefinition.SerializationSettings.Add(new QuestionJsonSerializationSettings(
				question => serializer((QuestionT) question),
				jsonType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures XML persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="QuestionT">Question type being configured.</typeparam>
		/// <typeparam name="XmlT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition<QuestionT> SerializeToXml<QuestionT, XmlT>(
			this QuestionDefinition<QuestionT> metadataDefinition,
			Func<QuestionT, XmlT> serializer)
			where QuestionT : IQuestion
			where XmlT : Serialization.Xml.Question
		{
			return metadataDefinition.SerializeToXml(
				serializer,
				typeof(XmlT));
		}

		/// <summary>
		/// Configures JSON persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="QuestionT">Question type being configured.</typeparam>
		/// <typeparam name="JsonT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts a question into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static QuestionDefinition<QuestionT> SerializeToJson<QuestionT, JsonT>(
			this QuestionDefinition<QuestionT> metadataDefinition,
			Func<QuestionT, JsonT> serializer)
			where QuestionT : IQuestion
			where JsonT : Serialization.Json.Question
		{
			return metadataDefinition.SerializeToJson(
				serializer,
				typeof(JsonT));
		}
	}
}