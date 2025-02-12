using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageExtensionQuestions
	{
		ILanguageQuestionNames Names
		{ get; }

		ILanguageQuestionParameters Parameters
		{ get; }

		ILanguageQuestionAnswers Answers
		{ get; }
	}

	[XmlType]
	public class LanguageExtensionQuestions : ILanguageExtensionQuestions
	{
		public ILanguageQuestionNames Names
		{ get; set; }

		public ILanguageQuestionParameters Parameters
		{ get; set; }

		public ILanguageQuestionAnswers Answers
		{ get; set; }
	}

	public interface ILanguageQuestionNames
	{ }

	[XmlType]
	public class LanguageQuestionNames : ILanguageQuestionNames
	{ }

	public interface ILanguageQuestionAnswers
	{ }

	[XmlType]
	public class LanguageQuestionAnswers : ILanguageQuestionAnswers
	{ }
}
