using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	/// <summary>The mathematics module's string bundle: its attribute, concept, statement and question texts.</summary>
	public interface ILanguageMathematicsModule : ILanguageAttributesExtension<ILanguageAttributes>, ILanguageConceptsExtension<ILanguageConcepts>, ILanguageStatementsExtension<ILanguageStatements>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

	/// <summary>Serializable <see cref="ILanguageMathematicsModule"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageMathematicsModule : LanguageExtension, ILanguageMathematicsModule
	{
		#region Xml Properties

		/// <summary>Attribute names contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		/// <summary>Statement wordings contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		/// <summary>Question wordings contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		/// <summary>Concept texts contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Concepts))]
		public LanguageConcepts ConceptsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		ILanguageExtensionAttributes ILanguageAttributesExtension.Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		ILanguageExtensionConcepts ILanguageConceptsExtension.Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		ILanguageExtensionStatements ILanguageStatementsExtension.Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		ILanguageExtensionQuestions ILanguageQuestionsExtension.Questions
		{ get { return QuestionsXml; } }

		/// <summary>Attribute names contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		/// <summary>Statement wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		/// <summary>Question wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		/// <summary>Concept texts contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		#endregion

		/// <summary>Builds the bundle with its built-in English texts.</summary>
		/// <returns>A fully populated bundle.</returns>
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
