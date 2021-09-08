using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Constants

		[XmlIgnore]
		private const String ElementQuestionNames = "QuestionNames";
		[XmlIgnore]
		private const String ElementAnswers = "Answers";

		#endregion

		#region Xml Properties

		[XmlElement(ElementQuestionNames)]
		public LanguageQuestionNames QuestionNamesXml
		{ get; set; }

		[XmlElement(ElementAnswers)]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageQuestionNames QuestionNames
		{ get { return QuestionNamesXml; } }

		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		#endregion

		internal static LanguageQuestions CreateDefault()
		{
			return new LanguageQuestions
			{
				QuestionNamesXml = LanguageQuestionNames.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
