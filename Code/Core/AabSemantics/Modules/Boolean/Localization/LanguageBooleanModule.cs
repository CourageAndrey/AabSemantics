using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>The boolean module's string bundle: its attribute names, concept texts and question names.</summary>
	public interface ILanguageBooleanModule : ILanguageAttributesExtension<ILanguageAttributes>, ILanguageConceptsExtension<ILanguageConcepts>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

	/// <summary>
	/// Serializable <see cref="ILanguageBooleanModule"/>. Each bundle is exposed three times: as
	/// the concrete <c>*Xml</c> property the serializer writes, as an explicit implementation of
	/// the untyped extension interface, and as the typed public property module code uses.
	/// </summary>
	[XmlType]
	public class LanguageBooleanModule : LanguageExtension, ILanguageBooleanModule
	{
		#region Xml Properties

		/// <summary>Attribute names, in serializable form.</summary>
		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		/// <summary>Concept texts, in serializable form.</summary>
		[XmlElement(nameof(Concepts))]
		public LanguageConcepts ConceptsXml
		{ get; set; }

		/// <summary>Question wordings, in serializable form.</summary>
		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
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
		ILanguageExtensionQuestions ILanguageQuestionsExtension.Questions
		{ get { return QuestionsXml; } }

		/// <summary>Attribute names contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		/// <summary>Concept texts contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		/// <summary>Question wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		/// <summary>Builds the bundle with its built-in English texts.</summary>
		/// <returns>A fully populated bundle.</returns>
		public static LanguageBooleanModule CreateDefault()
		{
			return new LanguageBooleanModule()
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				ConceptsXml = LanguageConcepts.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
