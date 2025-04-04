﻿using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	public interface ILanguageBooleanModule : ILanguageAttributesExtension<ILanguageAttributes>, ILanguageConceptsExtension<ILanguageConcepts>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

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
		ILanguageExtensionAttributes ILanguageAttributesExtension.Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		ILanguageExtensionConcepts ILanguageConceptsExtension.Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		ILanguageExtensionQuestions ILanguageQuestionsExtension.Questions
		{ get { return QuestionsXml; } }

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
