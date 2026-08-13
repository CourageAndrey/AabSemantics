using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>Question wordings contributed by the set module.</summary>
	public interface ILanguageQuestions : ILanguageExtensionQuestions
	{
		/// <summary>Display names.</summary>
		ILanguageQuestionNames Names
		{ get; }

		/// <summary>Captions of the question parameters.</summary>
		ILanguageQuestionParameters Parameters
		{ get; }

		/// <summary>Wordings of the module's answers.</summary>
		ILanguageAnswers Answers
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageQuestions"/>, loaded from a language file.</summary>
	[XmlType("SetsQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		/// <summary>Display names. In serializable form.</summary>
		[XmlElement(nameof(Names))]
		public LanguageQuestionNames NamesXml
		{ get; set; }

		/// <summary>Captions of the question parameters. In serializable form.</summary>
		[XmlElement(nameof(Parameters))]
		public LanguageQuestionParameters ParametersXml
		{ get; set; }

		/// <summary>Wordings of the module's answers. In serializable form.</summary>
		[XmlElement(nameof(Answers))]
		public LanguageAnswers AnswersXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Display names.</summary>
		[XmlIgnore]
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

		/// <summary>Captions of the question parameters.</summary>
		[XmlIgnore]
		public AabSemantics.Modules.Set.Localization.ILanguageQuestionParameters Parameters
		{ get { return ParametersXml; } }

		/// <summary>Wordings of the module's answers.</summary>
		[XmlIgnore]
		public AabSemantics.Modules.Set.Localization.ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageQuestions CreateDefault()
		{
			return new LanguageQuestions
			{
				NamesXml = LanguageQuestionNames.CreateDefault(),
				ParametersXml = LanguageQuestionParameters.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
