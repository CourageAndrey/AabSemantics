using System.Xml.Serialization;

using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	[XmlType]
	public class LanguageClassificationModule : LanguageExtension<LanguageExtensionAttributes, LanguageConcepts, LanguageStatements, LanguageQuestions>
	{
		public static LanguageClassificationModule CreateDefault()
		{
			return new LanguageClassificationModule
			{
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
