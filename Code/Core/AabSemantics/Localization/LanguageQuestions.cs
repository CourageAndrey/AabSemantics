using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageQuestions
	{
		String CustomStatementQuestionName
		{ get; }

		ILanguageQuestionParameters Parameters
		{ get; }

		ILanguageAnswers Answers
		{ get; }
	}

	[XmlType("CommonQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		[XmlElement(nameof(CustomStatementQuestionName))]
		public String CustomStatementQuestionName
		{ get; set; }

		[XmlElement(nameof(Parameters))]
		public LanguageQuestionParameters ParametersXml
		{ get; set; }

		[XmlElement(nameof(Answers))]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageQuestionParameters Parameters
		{ get { return ParametersXml; } }

		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		#endregion

		internal static LanguageQuestions CreateDefault()
		{
			return new LanguageQuestions
			{
				CustomStatementQuestionName = "CustomStatementQuestion",
				ParametersXml = LanguageQuestionParameters.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
