using System;

namespace Inventor.Semantics.Metadata
{
	public class QuestionJsonSerializationSettings : IQuestionSerializationSettings, IJsonSerializationSettings
	{
		public Type JsonType
		{ get; }

		private readonly Func<IQuestion, Serialization.Json.Question> _questionJsonGetter;

		public QuestionJsonSerializationSettings(
			Func<IQuestion, Serialization.Json.Question> questionJsonGetter,
			Type jsonType)
		{
			if (questionJsonGetter == null) throw new ArgumentNullException(nameof(questionJsonGetter));
			if (jsonType == null) throw new ArgumentNullException(nameof(jsonType));
			if (jsonType.IsAbstract || !typeof(Serialization.Json.Question).IsAssignableFrom(jsonType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Json.Question)}.", nameof(jsonType));

			_questionJsonGetter = questionJsonGetter;
			JsonType = jsonType;
		}

		public Serialization.Json.Question GetJson(IQuestion question)
		{
			return _questionJsonGetter(question);
		}
	}

	public class QuestionXmlSerializationSettings : IQuestionSerializationSettings, IXmlSerializationSettings
	{
		public Type XmlType
		{ get; }

		private readonly Func<IQuestion, Serialization.Xml.Question> _questionXmlGetter;

		public QuestionXmlSerializationSettings(
			Func<IQuestion, Serialization.Xml.Question> questionXmlGetter,
			Type xmlType)
		{
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Serialization.Xml.Question).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Xml.Question)}.", nameof(xmlType));

			_questionXmlGetter = questionXmlGetter;
			XmlType = xmlType;
		}

		public Serialization.Xml.Question GetXml(IQuestion question)
		{
			return _questionXmlGetter(question);
		}
	}

	public class QuestionDefinition : IMetadataDefinition<QuestionJsonSerializationSettings, QuestionXmlSerializationSettings>
	{
		#region Properties

		public Type Type
		{ get; }

		public QuestionJsonSerializationSettings JsonSerializationSettings
		{ get; }

		public QuestionXmlSerializationSettings XmlSerializationSettings
		{ get; }

		IJsonSerializationSettings IMetadataDefinition.JsonSerializationSettings
		{ get { return JsonSerializationSettings; } }

		IXmlSerializationSettings IMetadataDefinition.XmlSerializationSettings
		{ get { return XmlSerializationSettings; } }

		private readonly Func<ILanguage, String> _questionNameGetter;

		#endregion

		public QuestionDefinition(
			Type type,
			Func<ILanguage, String> questionNameGetter,
			Func<IQuestion, Serialization.Xml.Question> questionXmlGetter,
			Func<IQuestion, Serialization.Json.Question> questionJsonGetter,
			Type xmlType,
			Type jsonType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IQuestion).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IQuestion)}.", nameof(type));
			if (questionNameGetter == null) throw new ArgumentNullException(nameof(questionNameGetter));

			Type = type;
			_questionNameGetter = questionNameGetter;
			JsonSerializationSettings = new QuestionJsonSerializationSettings(questionJsonGetter, jsonType);
			XmlSerializationSettings = new QuestionXmlSerializationSettings(questionXmlGetter, xmlType);
		}

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}
	}

	public class QuestionDefinition<QuestionT> : QuestionDefinition
		where QuestionT : IQuestion
	{
		public QuestionDefinition(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, Serialization.Xml.Question> questionXmlGetter,
			Func<QuestionT, Serialization.Json.Question> questionJsonGetter,
			Type xmlType,
			Type jsonType)
			: base(
				typeof(QuestionT),
				questionNameGetter,
				question => questionXmlGetter((QuestionT) question),
				question => questionJsonGetter((QuestionT) question),
				xmlType,
				jsonType)
		{
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
			if (questionJsonGetter == null) throw new ArgumentNullException(nameof(questionJsonGetter));
		}
	}

	public class QuestionDefinition<QuestionT, XmlT, JsonT> : QuestionDefinition<QuestionT>
		where QuestionT : IQuestion
		where XmlT : Serialization.Xml.Question
		where JsonT : Serialization.Json.Question
	{
		public QuestionDefinition(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, XmlT> questionXmlGetter,
			Func<QuestionT, JsonT> questionJsonGetter)
			: base(
				questionNameGetter,
				questionXmlGetter,
				questionJsonGetter,
				typeof(XmlT),
				typeof(JsonT))
		{
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
			if (questionJsonGetter == null) throw new ArgumentNullException(nameof(questionJsonGetter));
		}
	}
}