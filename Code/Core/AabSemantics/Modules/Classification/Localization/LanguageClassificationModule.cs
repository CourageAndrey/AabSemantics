using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>The classification module's string bundle: its statement and question wordings.</summary>
	public interface ILanguageClassificationModule : ILanguageStatementsExtension<ILanguageStatements>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

	/// <summary>Serializable <see cref="ILanguageClassificationModule"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageClassificationModule : LanguageExtension, ILanguageClassificationModule
	{
		#region Xml Properties

		/// <summary>Statement wordings, in serializable form.</summary>
		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		/// <summary>Question wordings, in serializable form.</summary>
		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		ILanguageExtensionStatements ILanguageStatementsExtension.Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		ILanguageExtensionQuestions ILanguageQuestionsExtension.Questions
		{ get { return QuestionsXml; } }

		/// <summary>Statement wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		/// <summary>Question wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		/// <summary>Builds the bundle with its built-in English texts.</summary>
		/// <returns>A fully populated bundle.</returns>
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
