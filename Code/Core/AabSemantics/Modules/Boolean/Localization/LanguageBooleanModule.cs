using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	[XmlType]
	public class LanguageBooleanModule : LanguageExtension<LanguageAttributes, LanguageConcepts, LanguageStatements, LanguageQuestions>
	{
		public static LanguageBooleanModule CreateDefault()
		{
			return new LanguageBooleanModule
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				ConceptsXml = LanguageConcepts.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
