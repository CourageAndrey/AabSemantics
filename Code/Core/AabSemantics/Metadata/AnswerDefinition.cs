using System;

using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// JSON persistence settings for an answer type: the surrogate type and the function
	/// converting an answer into it.
	/// </summary>
	public class AnswerJsonSerializationSettings : IAnswerSerializationSettings, IJsonSerializationSettings
	{
		/// <summary>
		/// JSON surrogate type answers are converted into.
		/// </summary>
		public Type JsonType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Json.Answer> _serializer;

		/// <summary>
		/// Configures JSON persistence.
		/// </summary>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <param name="jsonType">Surrogate type; must derive from the JSON answer base type.</param>
		public AnswerJsonSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Json.Answer> serializer,
			Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Answer>(nameof(jsonType));
		}

		/// <summary>
		/// Converts an answer into its JSON surrogate.
		/// </summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Json.Answer GetJson(IAnswer answer, ILanguage language)
		{
			return _serializer(answer, language);
		}
	}

	/// <summary>
	/// XML persistence settings for an answer type: the surrogate type and the function
	/// converting an answer into it.
	/// </summary>
	public class AnswerXmlSerializationSettings : IAnswerSerializationSettings, IXmlSerializationSettings
	{
		/// <summary>
		/// XML surrogate type answers are converted into.
		/// </summary>
		public Type XmlType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Xml.Answer> _serializer;

		/// <summary>
		/// Configures XML persistence.
		/// </summary>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <param name="xmlType">Surrogate type; must derive from the XML answer base type.</param>
		public AnswerXmlSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> serializer,
			Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Answer>(nameof(xmlType));
		}

		/// <summary>
		/// Converts an answer into its XML surrogate.
		/// </summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		public Serialization.Xml.Answer GetXml(IAnswer answer, ILanguage language)
		{
			return _serializer(answer, language);
		}
	}

	/// <summary>
	/// Runtime description of an answer type. Answers need no behaviour beyond persistence,
	/// so the definition only carries serialization settings.
	/// </summary>
	public class AnswerDefinition : MetadataDefinition<IAnswerSerializationSettings>
	{
		/// <summary>
		/// Describes an answer type.
		/// </summary>
		/// <param name="type">Answer type being described; must implement <see cref="IAnswer"/>.</param>
		public AnswerDefinition(Type type)
			: base(type, typeof(IAnswer))
		{ }
	}

	/// <summary>
	/// Strongly typed <see cref="AnswerDefinition"/>, sparing the caller an explicit type argument.
	/// </summary>
	/// <typeparam name="AnswerT">Answer type being described.</typeparam>
	public class AnswerDefinition<AnswerT> : AnswerDefinition
		where AnswerT : IAnswer
	{
		/// <summary>
		/// Describes the answer type.
		/// </summary>
		public AnswerDefinition()
			: base(typeof(AnswerT))
		{ }
	}

	/// <summary>
	/// Fluent configuration of how an answer type is persisted.
	/// </summary>
	public static class AnswerDefinitionExtensions
	{
		/// <summary>
		/// Returns the definition's XML settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The XML settings.</returns>
		/// <exception cref="InvalidOperationException">XML serialization has not been configured.</exception>
		public static IXmlSerializationSettings GetXmlSerializationSettings(this AnswerDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<AnswerXmlSerializationSettings>();
		}

		/// <summary>
		/// Returns the definition's JSON settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The JSON settings.</returns>
		/// <exception cref="InvalidOperationException">JSON serialization has not been configured.</exception>
		public static IJsonSerializationSettings GetJsonSerializationSettings(this AnswerDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<AnswerJsonSerializationSettings>();
		}

		/// <summary>
		/// Configures XML persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="AnswerT">Answer type being configured.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <param name="xmlType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AnswerDefinition<AnswerT> SerializeToXml<AnswerT>(
			this AnswerDefinition<AnswerT> metadataDefinition,
			Func<AnswerT, ILanguage, Serialization.Xml.Answer> serializer,
			Type xmlType)
			where AnswerT : IAnswer
		{
			metadataDefinition.SerializationSettings.Add(new AnswerXmlSerializationSettings(
				(answer, language) => serializer((AnswerT) answer, language),
				xmlType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence with an explicitly supplied surrogate type.
		/// </summary>
		/// <typeparam name="AnswerT">Answer type being configured.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <param name="jsonType">Surrogate type.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AnswerDefinition<AnswerT> SerializeToJson<AnswerT>(
			this AnswerDefinition<AnswerT> metadataDefinition,
			Func<AnswerT, ILanguage, Serialization.Json.Answer> serializer,
			Type jsonType)
			where AnswerT : IAnswer
		{
			metadataDefinition.SerializationSettings.Add(new AnswerJsonSerializationSettings(
				(answer, language) => serializer((AnswerT) answer, language),
				jsonType));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures XML persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="AnswerT">Answer type being configured.</typeparam>
		/// <typeparam name="XmlT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AnswerDefinition<AnswerT> SerializeToXml<AnswerT, XmlT>(
			this AnswerDefinition<AnswerT> metadataDefinition,
			Func<AnswerT, ILanguage, XmlT> serializer)
			where AnswerT : IAnswer
			where XmlT : Serialization.Xml.Answer
		{
			return metadataDefinition.SerializeToXml(
				serializer,
				typeof(XmlT));
		}

		/// <summary>
		/// Configures JSON persistence, inferring the surrogate type from the type argument.
		/// </summary>
		/// <typeparam name="AnswerT">Answer type being configured.</typeparam>
		/// <typeparam name="JsonT">Surrogate type.</typeparam>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="serializer">Converts an answer into its surrogate.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AnswerDefinition<AnswerT> SerializeToJson<AnswerT, JsonT>(
			this AnswerDefinition<AnswerT> metadataDefinition,
			Func<AnswerT, ILanguage, JsonT> serializer)
			where AnswerT : IAnswer
			where JsonT : Serialization.Json.Answer
		{
			return metadataDefinition.SerializeToJson(
				serializer,
				typeof(JsonT));
		}
	}
}
