using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	[XmlType]
	public class LanguageMathematicsModule : LanguageExtension<LanguageAttributes, LanguageConcepts, LanguageStatements, LanguageQuestions>
	{
		public static LanguageMathematicsModule CreateDefault()
		{
			return new LanguageMathematicsModule
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				ConceptsXml = LanguageConcepts.CreateDefault(),
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
