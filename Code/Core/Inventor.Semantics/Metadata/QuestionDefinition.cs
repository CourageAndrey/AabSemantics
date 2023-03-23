using System;

namespace Inventor.Semantics.Metadata
{
	public class QuestionDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		public Type XmlType
		{ get; }

		public Type JsonType
		{ get; }

		private readonly Func<ILanguage, String> _questionNameGetter;
		private readonly Func<IQuestion, Serialization.Xml.Question> _questionXmlGetter;
		private readonly Func<IQuestion, Serialization.Json.Question> _questionJsonGetter;

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
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
			if (questionJsonGetter == null) throw new ArgumentNullException(nameof(questionJsonGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Serialization.Xml.Question).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Xml.Question)}.", nameof(xmlType));
			if (jsonType == null) throw new ArgumentNullException(nameof(jsonType));
			if (jsonType.IsAbstract || !typeof(Serialization.Json.Question).IsAssignableFrom(jsonType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Json.Question)}.", nameof(jsonType));

			Type = type;
			_questionNameGetter = questionNameGetter;
			_questionXmlGetter = questionXmlGetter;
			_questionJsonGetter = questionJsonGetter;
			XmlType = xmlType;
			JsonType = jsonType;
		}

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}

		public Serialization.Xml.Question GetXml(IQuestion question)
		{
			return _questionXmlGetter(question);
		}

		public Serialization.Json.Question GetJson(IQuestion question)
		{
			return _questionJsonGetter(question);
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