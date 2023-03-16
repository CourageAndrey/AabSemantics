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

		private readonly Func<IAnswer, ILanguage, Xml.Answer> _answerXmlGetter;

		#endregion

		#region Constructors

		public AnswerDefinition(
			Type type,
			Func<IAnswer, ILanguage, Xml.Answer> answerXmlGetter,
			Type xmlType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IAnswer).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IAnswer)}.", nameof(type));
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
			if (xmlType == null) throw new ArgumentNullException(nameof(xmlType));

			Type = type;
			_answerXmlGetter = answerXmlGetter;
			XmlType = xmlType;;
		}

		#endregion

		public Xml.Answer GetXml(IAnswer answer, ILanguage language)
		{
			return _answerXmlGetter(answer, language);
		}
	}

	public class AnswerDefinition<AnswerT> : AnswerDefinition
		where AnswerT : IAnswer
	{
		public AnswerDefinition(
			Func<AnswerT, ILanguage, Xml.Answer> answerXmlGetter,
			Type xmlType)
			: base(
				typeof(AnswerT),
				(answer, language) => answerXmlGetter((AnswerT) answer, language),
				xmlType)
		{
			if (answerXmlGetter == null) throw new ArgumentNullException(nameof(answerXmlGetter));
		}
	}
}
