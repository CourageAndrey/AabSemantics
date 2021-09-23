using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules
{
	public interface ILanguageProcessesModule
	{
		Processes.ILanguageAttributes Attributes
		{ get; }

		Processes.ILanguageStatements Statements
		{ get; }

		Processes.ILanguageQuestions Questions
		{ get; }

		Processes.ILanguageConcepts Concepts
		{ get; }
	}

	[XmlType]
	public class LanguageProcessesModule : LanguageExtension, ILanguageProcessesModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public Processes.LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public Processes.LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public Processes.LanguageQuestions QuestionsXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public Processes.LanguageConcepts ConceptsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public Processes.ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public Processes.ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public Processes.ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public Processes.ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		#endregion

		public static LanguageProcessesModule CreateDefault()
		{
			return new LanguageProcessesModule()
			{
				AttributesXml = Processes.LanguageAttributes.CreateDefault(),
				ConceptsXml = Processes.LanguageConcepts.CreateDefault(),
				StatementsXml = Processes.LanguageStatements.CreateDefault(),
				QuestionsXml = Processes.LanguageQuestions.CreateDefault(),
			};
		}
	}
}
