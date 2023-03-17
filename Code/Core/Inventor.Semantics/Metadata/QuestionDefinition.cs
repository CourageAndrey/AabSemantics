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

		private readonly Func<ILanguage, String> _questionNameGetter;
		private readonly Func<IQuestion, Xml.Question> _questionXmlGetter;

		#endregion

		public QuestionDefinition(
			Type type,
			Func<ILanguage, String> questionNameGetter,
			Func<IQuestion, Xml.Question> questionXmlGetter,
			Type xmlType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IQuestion).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IQuestion)}.", nameof(type));
			if (questionNameGetter == null) throw new ArgumentNullException(nameof(questionNameGetter));
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));
			if (xmlType.IsAbstract || !typeof(Xml.Question).IsAssignableFrom(xmlType)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(Xml.Question)}.", nameof(xmlType));

			Type = type;
			_questionNameGetter = questionNameGetter;
			_questionXmlGetter = questionXmlGetter;
			XmlType = xmlType;
		}

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}

		public Xml.Question GetXml(IQuestion question)
		{
			return _questionXmlGetter(question);
		}
	}

	public class QuestionDefinition<QuestionT> : QuestionDefinition
		where QuestionT : IQuestion
	{
		public QuestionDefinition(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, Xml.Question> questionXmlGetter,
			Type xmlType)
			: base(
				typeof(QuestionT),
				questionNameGetter,
				question => questionXmlGetter((QuestionT) question),
				xmlType)
		{
			if (questionXmlGetter == null) throw new ArgumentNullException(nameof(questionXmlGetter));
		}
	}
}