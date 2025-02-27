using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	public interface ILanguageClassificationModule : ILanguageExtension
	{
		ILanguageStatements Statements
		{ get; }

		ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageClassificationModule : LanguageExtension, ILanguageClassificationModule
	{
		#region Xml Properties

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageClassificationModule CreateDefault()
		{
			return new LanguageClassificationModule()
			{
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
