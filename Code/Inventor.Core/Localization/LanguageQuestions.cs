using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Constants

		[XmlIgnore]
		private const String ElementQuestionNames = "Names";
		[XmlIgnore]
		private const String ElementAnswers = "Answers";

		#endregion

		#region Xml Properties

		[XmlElement(ElementQuestionNames)]
		public LanguageQuestionNames NamesXml
		{ get; set; }

		[XmlElement(ElementAnswers)]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		#endregion

		internal static LanguageQuestions CreateDefault()
		{
			return new LanguageQuestions
			{
				NamesXml = LanguageQuestionNames.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
