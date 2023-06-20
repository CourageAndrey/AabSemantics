using System;

using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	public class QuestionJsonSerializationSettings : IQuestionSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IQuestion, Serialization.Json.Question> _serializer;

		public QuestionJsonSerializationSettings(
			Func<IQuestion, Serialization.Json.Question> serializer,
			Type jsonType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			JsonType = jsonType.EnsureNotNull(nameof(jsonType)).EnsureContract<Serialization.Json.Question>(nameof(jsonType));
		}

		public Serialization.Json.Question GetJson(IQuestion question)
		{
			return _serializer(question);
		}
	}

	public class QuestionXmlSerializationSettings : IQuestionSerializationSettings, IXmlSerializationSettings
	{
		public Type XmlType
		{ get; }

		private readonly Func<IQuestion, Serialization.Xml.Question> _serializer;

		public QuestionXmlSerializationSettings(
			Func<IQuestion, Serialization.Xml.Question> serializer,
			Type xmlType)
		{
			_serializer = serializer.EnsureNotNull(nameof(serializer));
			XmlType = xmlType.EnsureNotNull(nameof(xmlType)).EnsureContract<Serialization.Xml.Question>(nameof(xmlType));
		}

		public Serialization.Xml.Question GetXml(IQuestion question)
		{
			return _serializer(question);
		}
	}

	public class QuestionDefinition : MetadataDefinition<IQuestionSerializationSettings>
	{
		#region Properties

		private readonly Func<ILanguage, String> _nameGetter;

		#endregion

		public QuestionDefinition(Type type, Func<ILanguage, String> nameGetter)
			: base(type, typeof(IQuestion))
		{
			_nameGetter = nameGetter.EnsureNotNull(nameof(nameGetter));
		}

		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}
	}

	public class QuestionDefinition<QuestionT> : QuestionDefinition
		where QuestionT : IQuestion
	{
		public QuestionDefinition(Func<ILanguage, String> nameGetter)
			: base(typeof(QuestionT), nameGetter)
		{ }
	}

	public static class QuestionDefinitionExtensions
	{
		public static IXmlSerializationSettings GetXmlSerializationSettings(this QuestionDefinition metadataDefinition)
		{
			return metadataDefinition.GetXmlSerializationSettings<QuestionXmlSerializationSettings>();
		}

		public static SettingsT GetXmlSerializationSettings<SettingsT>(this QuestionDefinition metadataDefinition)
			where SettingsT : IXmlSerializationSettings, IQuestionSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static IJsonSerializationSettings GetJsonSerializationSettings(this QuestionDefinition metadataDefinition)
		{
			return metadataDefinition.GetJsonSerializationSettings<QuestionJsonSerializationSettings>();
		}

		public static SettingsT GetJsonSerializationSettings<SettingsT>(this QuestionDefinition metadataDefinition)
			where SettingsT : IJsonSerializationSettings, IQuestionSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static QuestionDefinition SerializeToXml(
			this QuestionDefinition metadataDefinition,
			Func<IQuestion, Serialization.Xml.Question> serializer,
			Type xmlType)
		{
			metadataDefinition.SerializationSettings.Add(new QuestionXmlSerializationSettings(serializer, xmlType));
			return metadataDefinition;
		}

		public static QuestionDefinition SerializeToJson(
			this QuestionDefinition metadataDefinition,
			Func<IQuestion, Serialization.Json.Question> questionJsonGetter,
			Type jsonType)
		{
			metadataDefinition.SerializationSettings.Add(new QuestionJsonSerializationSettings(questionJsonGetter, jsonType));
			return metadataDefinition;
		}

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