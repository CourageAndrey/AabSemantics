using System;
using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>Wordings for the built-in questions, their parameters and their answers.</summary>
	public interface ILanguageQuestions
	{
		/// <summary>Display name of the custom-statement question.</summary>
		String CustomStatementQuestionName
		{ get; }

		/// <summary>Captions of the parameters a question asks the user for.</summary>
		ILanguageQuestionParameters Parameters
		{ get; }

		/// <summary>Wordings used when rendering answers.</summary>
		ILanguageAnswers Answers
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestions"/>, loaded from a language file.</summary>
	[XmlType("CommonQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		/// <summary>Display name of the custom-statement question.</summary>
		[XmlElement(nameof(CustomStatementQuestionName))]
		public String CustomStatementQuestionName
		{ get; set; }

		/// <summary>Parameter captions, in serializable form.</summary>
		[XmlElement(nameof(Parameters))]
		public LanguageQuestionParameters ParametersXml
		{ get; set; }

		/// <summary>Answer wordings, in serializable form.</summary>
		[XmlElement(nameof(Answers))]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Parameter captions.</summary>
		[XmlIgnore]
		public ILanguageQuestionParameters Parameters
		{ get { return ParametersXml; } }

		/// <summary>Answer wordings.</summary>
		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
