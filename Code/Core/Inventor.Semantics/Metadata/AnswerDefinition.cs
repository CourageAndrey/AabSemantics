using System;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Metadata
{
	public class AnswerJsonSerializationSettings : IAnswerSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Json.Answer> _serializer;

		public AnswerJsonSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Json.Answer> serializer,
			Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Answer>(nameof(jsonType));
		}

		public Serialization.Json.Answer GetJson(IAnswer answer, ILanguage language)
		{
			return _serializer(answer, language);
		}
	}

	public class AnswerXmlSerializationSettings : IAnswerSerializationSettings, IXmlSerializationSettings
	{
		public Type XmlType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Xml.Answer> _serializer;

		public AnswerXmlSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> serializer,
			Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Answer>(nameof(xmlType));
		}

		public Serialization.Xml.Answer GetXml(IAnswer answer, ILanguage language)
		{
			return _serializer(answer, language);
		}
	}

	public class AnswerDefinition : MetadataDefinition<IAnswerSerializationSettings>
	{
		public AnswerDefinition(Type type)
			: base(type, typeof(IAnswer))
		{ }
	}

	public class AnswerDefinition<AnswerT> : AnswerDefinition
		where AnswerT : IAnswer
	{
		public AnswerDefinition()
			: base(typeof(AnswerT))
		{ }
	}

	public static class AnswerDefinitionExtensions
	{
		public static IXmlSerializationSettings GetXmlSerializationSettings(this AnswerDefinition metadataDefinition)
		{
			return metadataDefinition.GetXmlSerializationSettings<AnswerXmlSerializationSettings>();
		}

		public static SettingsT GetXmlSerializationSettings<SettingsT>(this AnswerDefinition metadataDefinition)
			where SettingsT : IXmlSerializationSettings, IAnswerSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static IJsonSerializationSettings GetJsonSerializationSettings(this AnswerDefinition metadataDefinition)
		{
			return metadataDefinition.GetJsonSerializationSettings<AnswerJsonSerializationSettings>();
		}

		public static SettingsT GetJsonSerializationSettings<SettingsT>(this AnswerDefinition metadataDefinition)
			where SettingsT : IJsonSerializationSettings, IAnswerSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

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
