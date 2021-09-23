using System.Xml.Serialization;

namespace Inventor.Core.Localization.Modules
{
	public interface ILanguageMathematicsModule
	{
		Mathematics.ILanguageAttributes Attributes
		{ get; }

		Mathematics.ILanguageStatements Statements
		{ get; }

		Mathematics.ILanguageQuestions Questions
		{ get; }

		Mathematics.ILanguageConcepts Concepts
		{ get; }
	}

	[XmlType]
	public class LanguageMathematicsModule : LanguageExtension, ILanguageMathematicsModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public Mathematics.LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public Mathematics.LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public Mathematics.LanguageQuestions QuestionsXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public Mathematics.LanguageConcepts ConceptsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public Mathematics.ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public Mathematics.ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public Mathematics.ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public Mathematics.ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		#endregion

		public static LanguageMathematicsModule CreateDefault()
		{
			return new LanguageMathematicsModule()
			{
				AttributesXml = Mathematics.LanguageAttributes.CreateDefault(),
				ConceptsXml = Mathematics.LanguageConcepts.CreateDefault(),
				StatementsXml = Mathematics.LanguageStatements.CreateDefault(),
				QuestionsXml = Mathematics.LanguageQuestions.CreateDefault(),
			};
		}
	}
}
