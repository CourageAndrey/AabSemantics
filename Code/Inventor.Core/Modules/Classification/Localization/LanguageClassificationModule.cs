using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules
{
	public interface ILanguageClassificationModule
	{
		Classification.ILanguageStatements Statements
		{ get; }

		Classification.ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageClassificationModule : LanguageExtension, ILanguageClassificationModule
	{
		#region Xml Properties

		[XmlElement(nameof(Statements))]
		public Classification.LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public Classification.LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public Classification.ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public Classification.ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageClassificationModule CreateDefault()
		{
			return new LanguageClassificationModule()
			{
				StatementsXml = Classification.LanguageStatements.CreateDefault(),
				QuestionsXml = Classification.LanguageQuestions.CreateDefault(),
			};
		}
	}
}
