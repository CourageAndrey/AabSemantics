using System.Xml.Serialization;

namespace Inventor.Semantics.Localization.Modules
{
	public interface ILanguageBooleanModule
	{
		Boolean.ILanguageAttributes Attributes
		{ get; }

		Boolean.ILanguageConcepts Concepts
		{ get; }

		Boolean.ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageBooleanModule : LanguageExtension, ILanguageBooleanModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public Boolean.LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public Boolean.LanguageConcepts ConceptsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public Boolean.LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public Boolean.ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public Boolean.ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		public Boolean.ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageBooleanModule CreateDefault()
		{
			return new LanguageBooleanModule()
			{
				AttributesXml = Boolean.LanguageAttributes.CreateDefault(),
				ConceptsXml = Boolean.LanguageConcepts.CreateDefault(),
				QuestionsXml = Boolean.LanguageQuestions.CreateDefault(),
			};
		}
	}
}
