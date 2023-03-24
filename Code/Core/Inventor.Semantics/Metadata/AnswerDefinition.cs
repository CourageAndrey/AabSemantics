using System;

namespace Inventor.Semantics.Metadata
{
	public class AnswerJsonSerializationSettings : IAnswerSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Json.Answer> _answerJsonGetter;

		public AnswerJsonSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type jsonType)
		{
			if (answerJsonGetter == null) throw new ArgumentNullException(nameof(answerJsonGetter));
			if (jsonType == null) throw new ArgumentNullException(nameof(jsonType));
			if (jsonType.IsAbstract || !typeof(Serialization.Json.Answer).IsAssignableFrom(jsonType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Json.Answer)}.", nameof(jsonType));

			_answerJsonGetter = answerJsonGetter;
			JsonType = jsonType;
		}

		public Serialization.Json.Answer GetJson(IAnswer answer, ILanguage language)
		{
			return _answerJsonGetter(answer, language);
		}
	}

	public class AnswerXmlSerializationSettings : IAnswerSerializationSettings, IXmlSerializationSettings
	{
		public Type XmlType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Xml.Answer> _answerXmlGetter;

		public AnswerXmlSerializationSettings(
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Type xmlType)
		{
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Serialization.Xml.Answer).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Xml.Answer)}.", nameof(xmlType));

			_answerXmlGetter = answerXmlGetter;
			XmlType = xmlType;
		}

		public Serialization.Xml.Answer GetXml(IAnswer answer, ILanguage language)
		{
			return _answerXmlGetter(answer, language);
		}
	}

	public class AnswerDefinition : MetadataDefinition<IAnswerSerializationSettings>
	{
		#region Constructors

		public AnswerDefinition(
			Type type,
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Func<IAnswer, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type xmlType,
			Type jsonType)
			: base(type, typeof(IAnswer))
		{
			SerializationSettings.Add(new AnswerJsonSerializationSettings(answerJsonGetter, jsonType));
			SerializationSettings.Add(new AnswerXmlSerializationSettings(answerXmlGetter, xmlType));
		}

		#endregion
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
	}

	public class AnswerDefinition<AnswerT> : AnswerDefinition
		where AnswerT : IAnswer
	{
		public AnswerDefinition(
			Func<AnswerT, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Func<AnswerT, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type xmlType,
			Type jsonType)
			: base(
				typeof(AnswerT),
				(answer, language) => answerXmlGetter((AnswerT) answer, language),
				(answer, language) => answerJsonGetter((AnswerT) answer, language),
				xmlType,
				jsonType)
		{
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
			if (answerJsonGetter == null) throw new ArgumentNullException(nameof(answerJsonGetter));
		}
	}

	public class AnswerDefinition<AnswerT, XmlT, JsonT> : AnswerDefinition<AnswerT>
		where AnswerT : IAnswer
		where XmlT : Serialization.Xml.Answer
		where JsonT : Serialization.Json.Answer
	{
		public AnswerDefinition(
			Func<AnswerT, ILanguage, XmlT> answerXmlGetter,
			Func<AnswerT, ILanguage, JsonT> answerJsonGetter)
			: base(
				answerXmlGetter,
				answerJsonGetter,
				typeof(XmlT),
				typeof(JsonT))
		{
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
			if (answerJsonGetter == null) throw new ArgumentNullException(nameof(answerJsonGetter));
		}
	}
}
