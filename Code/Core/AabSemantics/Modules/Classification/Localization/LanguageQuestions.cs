using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>Question wordings contributed by the classification module.</summary>
	public interface ILanguageQuestions : ILanguageExtensionQuestions
	{
		/// <summary>Display names of the module's questions.</summary>
		ILanguageQuestionNames Names
		{ get; }

		/// <summary>Wordings of the module's answers.</summary>
		ILanguageAnswers Answers
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestions"/>, loaded from a language file.</summary>
	[XmlType("ClassificationQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		/// <summary>Question names, in serializable form.</summary>
		[XmlElement(nameof(Names))]
		public LanguageQuestionNames NamesXml
		{ get; set; }

		/// <summary>Answer wordings, in serializable form.</summary>
		[XmlElement(nameof(Answers))]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Display names of the module's questions.</summary>
		[XmlIgnore]
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

		/// <summary>Wordings of the module's answers.</summary>
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
				NamesXml = LanguageQuestionNames.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
