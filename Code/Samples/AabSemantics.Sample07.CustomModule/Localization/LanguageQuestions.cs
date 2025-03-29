﻿using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Sample07.CustomModule.Localization
{
	public interface ILanguageQuestions : ILanguageExtensionQuestions
	{
		ILanguageQuestionNames Names
		{ get; }

		ILanguageQuestionParameters Parameters
		{ get; }

		ILanguageAnswers Answers
		{ get; }
	}

	[XmlType("CustomQuestions")]
	public class LanguageQuestions : ILanguageQuestions
	{
		#region Xml Properties

		[XmlElement(nameof(Names))]
		public LanguageQuestionNames NamesXml
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
		public ILanguageQuestionNames Names
		{ get { return NamesXml; } }

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
				NamesXml = LanguageQuestionNames.CreateDefault(),
				ParametersXml = LanguageQuestionParameters.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
			};
		}
	}
}
