using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Modules.Boolean.Localization
{
	public interface ILanguageBooleanModule
	{
		ILanguageAttributes Attributes
		{ get; }

		ILanguageConcepts Concepts
		{ get; }

		ILanguageQuestions Questions
		{ get; }
	}

	[XmlType]
	public class LanguageBooleanModule : LanguageExtension, ILanguageBooleanModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public LanguageConcepts ConceptsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageConcepts Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

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
