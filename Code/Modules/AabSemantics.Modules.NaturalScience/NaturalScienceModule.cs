using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.NaturalScience.Attributes;
using AabSemantics.Modules.NaturalScience.Concepts;
using AabSemantics.Modules.NaturalScience.Localization;
using AabSemantics.Modules.Set.Statements;
//using AabSemantics.Modules.NaturalScience.Questions;
//using AabSemantics.Modules.NaturalScience.Statements;
using AabSemantics.Serialization;
using AabSemantics.Statements;
using System;
using System.Collections.Generic;

namespace AabSemantics.Modules.NaturalScience
{
	public class NaturalScienceModule : ExtensionModule
	{
		public const String ModuleName = "System.NaturalSciense";

		public NaturalScienceModule()
			: base(ModuleName)
		{
			Science = new Concept(
				nameof(Science),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.Science),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.Science));
			Physics = new Concept(
				$"{nameof(Science)}.{nameof(Physics)}",
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.Physics),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.Physics));
			Chemistry = new Concept(
				$"{nameof(Science)}.{nameof(Chemistry)}",
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.Chemistry),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.Chemistry));
			Astronomy = new Concept(
				$"{nameof(Science)}.{nameof(Astronomy)}",
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.Astronomy),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.Astronomy));
			Biology = new Concept(
				$"{nameof(Science)}.{nameof(Biology)}",
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.Biology),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.Biology));
			ChemicalElement = new Concept(
				$"{nameof(Science)}.{nameof(ChemicalElement)}",
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptNames.ChemicalElement),
				new LocalizedStringConstant(language => language.GetExtension<ILanguageNaturalScienceModule>().Concepts.SystemConceptHints.ChemicalElement));
		}

		#region Properties

		public IConcept Science
		{ get; private set; }

		public IConcept Physics
		{ get; private set; }

		public IConcept Chemistry
		{ get; private set; }

		public IConcept Astronomy
		{ get; private set; }

		public IConcept Biology
		{ get; private set; }

		public IConcept ChemicalElement
		{ get; private set; }

		#endregion

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			var allScience = new[]
			{
				Physics,
				Chemistry,
				Astronomy,
				Biology,
			};
			var allConcepts = new List<IConcept>(allScience)
			{
				Science,
				ChemicalElement,
			};			
			foreach (var concept in allConcepts)
			{
				semanticNetwork.Concepts.Add(concept);
				ConceptIdResolver.SystemConceptsById[concept.ID] = concept;
			}

			semanticNetwork.DeclareThat(Science).IsSubjectAreaOf(allScience);
			semanticNetwork.DeclareThat(Science).HasParts(allScience);
			semanticNetwork.DeclareThat(Science).IsAncestorOf(allScience);

			semanticNetwork.DeclareThat(ChemicalElement).BelongsToSubjectAreas(new[] { Physics, Chemistry });
			foreach (var element in ChemicalElements.BySymbol.Values)
			{
				semanticNetwork.Concepts.Add(element);
				semanticNetwork.DeclareThat(element).IsDescendantOf(ChemicalElement);
			}
		}

		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(LanguageNaturalScienceModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
#warning SHIT
			/*Repositories.RegisterAttribute(IsChemicalElementAttribute.Value, language => language.GetExtension<ILanguageNaturalScienceModule>().Attributes.IsChemicalElement)
				.SerializeToXml(new Xml.IsChemicalElementAttribute())
				.SerializeToJson(new Xml.IsChemicalElementAttribute());*/
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(ChemicalElements));
		}

		//protected override void RegisterStatements()
		//{
		//}

		//protected override void RegisterQuestions()
		//{
		//}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(NaturalScienceModule), typeof(LanguageNaturalScienceModule) }
			};
		}
	}
}
