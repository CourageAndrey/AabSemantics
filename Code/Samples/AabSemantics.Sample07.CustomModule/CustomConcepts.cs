using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Sample07.CustomModule.Localization;

namespace AabSemantics.Sample07.CustomModule
{
	internal static class CustomConcepts
	{
		public static readonly IConcept Custom = new SystemConcept(
			"{Custom.Custom}",
			new LocalizedStringConstant(language => language.GetConceptsExtension<ILanguageCustomModule, ILanguageConcepts>().CustomName),
			new LocalizedStringConstant(language => language.GetConceptsExtension<ILanguageCustomModule, ILanguageConcepts>().CustomHint));
	}
}
