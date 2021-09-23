using System.Xml.Serialization;

using Inventor.Core.Localization;

namespace Inventor.Mathematics.Localization
{
	public interface ILanguageMathematicsModule
	{
		ILanguageAttributes Attributes
		{ get; }

		ILanguageStatements Statements
		{ get; }

		ILanguageQuestions Questions
		{ get; }

		ILanguageConcepts Concepts
		{ get; }
	}

	[XmlType]
	public class LanguageMathematicsModule : LanguageExtension, ILanguageMathematicsModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public LanguageConcepts ConceptsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		#endregion

		public static LanguageMathematicsModule CreateDefault()
		{
			return new LanguageMathematicsModule()
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				ConceptsXml = LanguageConcepts.CreateDefault(),
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
