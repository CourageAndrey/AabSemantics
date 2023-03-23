using System;

namespace Inventor.Semantics.Metadata
{
	public class AnswerDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		public Type XmlType
		{ get; }

		public Type JsonType
		{ get; }

		private readonly Func<IAnswer, ILanguage, Serialization.Xml.Answer> _answerXmlGetter;
		private readonly Func<IAnswer, ILanguage, Serialization.Json.Answer> _answerJsonGetter;

		#endregion

		#region Constructors

		public AnswerDefinition(
			Type type,
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Func<IAnswer, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type xmlType,
			Type jsonType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IAnswer).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IAnswer)}.", nameof(type));
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
			if (answerJsonGetter == null) throw new ArgumentNullException(nameof(answerJsonGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Serialization.Xml.Answer).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Xml.Answer)}.", nameof(xmlType));
			if (jsonType == null) throw new ArgumentNullException(nameof(jsonType));
			if (jsonType.IsAbstract || !typeof(Serialization.Json.Answer).IsAssignableFrom(jsonType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Serialization.Json.Answer)}.", nameof(jsonType));

			Type = type;
			_answerXmlGetter = answerXmlGetter;
			_answerJsonGetter = answerJsonGetter;
			XmlType = xmlType;
			JsonType = jsonType;
		}

		#endregion

		public Serialization.Xml.Answer GetXml(IAnswer answer, ILanguage language)
		{
			return _answerXmlGetter(answer, language);
		}

		public Serialization.Json.Answer GetJson(IAnswer answer, ILanguage language)
		{
			return _answerJsonGetter(answer, language);
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
